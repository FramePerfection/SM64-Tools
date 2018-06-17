using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter
{
    public class DisplayList
    {
        public class Light
        {
            public int color = (int)-1, ambient = 0x7f7f7fFF;
            public int direction = 0x66666600;
        }

        public Subset[] subsets = new Subset[0];
        public Command[] commands;
        public RenderStates renderstates = new RenderStates();
        public Light[] lightValues = new Light[] { new Light() };

        //Commands that are supposed to bring the rendering engine back into the state that the repsective render layer expects them to be in.
        //Layer is the first index.
        public static Command[][] defaultCommands = new Command[][] {
                                        null,
                                        new Command[] {new Command(0xB9, 0x31D, (int)0x00442078), new Command(0xFC, 0xFFFFFF, 0xFFFE793C),  new DisplayList.Command(0xBB, 0x0,0)},
                                        null,
                                        null,
                                        new Command[] {new Command(0xFC, 0xFFFFFF, 0xFFFE793C),  new DisplayList.Command(0xBB, 0x0,0)},
                                        new Command[] {new Command(0xB9, 0x31D, (int)0x005049D8), new Command(0xFC, 0xFFFFFF, 0xFFFE793C),  new DisplayList.Command(0xBB, 0x0,0)},
                                        new Command[] {new Command(0xB9, 0x31D, (int)0x00504DD8), new Command(0xFC, 0xFFFFFF, 0xFFFE793C),  new DisplayList.Command(0xBB, 0x0,0)}
        };

        public void ClearSubsets()
        {
            subsets = new Subset[0];
        }

        public void BuildCommands(int segmentOffset, int layer)
        {
            Dictionary<TextureInfo, List<Subset>> subsetGroups = new Dictionary<TextureInfo, List<Subset>>();
            foreach (Subset subset in subsets)
            {
                List<Subset> group;
                if (!subsetGroups.TryGetValue(subset.Texture, out group))
                    group = subsetGroups[subset.Texture] = new List<Subset>();
                group.Add(subset);
            }

            List<Command> cmd = new List<Command>();
            renderstates.AppendCommands(cmd, layer);
            foreach (KeyValuePair<TextureInfo, List<Subset>> subsetGroup in subsetGroups)
            {
                subsetGroup.Key.AppendCommands(cmd, segmentOffset, renderstates);
                foreach (Subset subset in subsetGroup.Value)
                    subset.AppendCommands(cmd, renderstates);
                subsetGroup.Key.AppendResetCommands(cmd);
            }

            //restore default render states and end DL
            unchecked
            {
                cmd.Add(new DisplayList.Command(0xE7));
                renderstates.Reset(cmd, layer);
                cmd.Add(new Command(0xB8, 0, 0));
            }
            commands = cmd.ToArray();
        }

        public class Command
        {
            public byte[] values = new byte[8];
            public Command(byte cmd, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
            {
                values = new byte[] { cmd, b1, b2, b3, b4, b5, b6, b7 };
            }
            public Command(byte cmd, int high, int low)
                : this(cmd, (byte)((high & 0xFF0000) >> 0x10), (byte)((high & 0xFF00) >> 0x8), (byte)(high & 0xFF), (byte)(low >> 0x18), (byte)((low & 0xFF0000) >> 0x10), (byte)((low & 0xFF00) >> 0x8), (byte)(low & 0xFF))
            { }
            public Command(byte cmd, uint high, uint low) : this(cmd, (int)high, (int)low) { }
            public Command(byte cmd) : this(cmd, 0, 0) { }

            public override string ToString()
            {
                StringBuilder ack = new StringBuilder();
                for (int i = 0; i < values.Length; i++)
                    ack.Append(values[i].ToString("X2") + " ");
                return ack.ToString();
            }
        }
    }

    public class Subset
    {
        public const int MAX_VERTICES = 15; //Maximum amount of vertices in a vertex buffer in F3D. IIRC, 16 has caused issues for me before.

        public Vertex[] VertexBuffer;
        public int[] IndexBuffer;
        public TrianglePatch[] Patches;
        public TextureInfo Texture;

        public void AppendCommands(List<DisplayList.Command> targetList, RenderStates states)
        {
            foreach (TrianglePatch patch in Patches)
            {
                int bufferSize = patch.numVertices * 0x10;
                targetList.Add(new DisplayList.Command(0x04, (Math.Min(0xF0, bufferSize) << 0x10) | bufferSize, patch.segmentedPointer));
                foreach (Triangle t in patch.triangles)
                    targetList.Add(new DisplayList.Command(0xBF, 0, 0, 0, 0, (byte)(t.v1 * 0xA), (byte)(t.v2 * 0xA), (byte)(t.v3 * 0xA)));
            }
        }

        public void CreatePatches()
        {
            TrianglePatch currentPatch = new TrianglePatch();
            Triangle currentTriangle = new Triangle();
            List<TrianglePatch> patches = new List<TrianglePatch>();
            for (int i = 0; i < IndexBuffer.Length; i++)
            {
                Vertex v = VertexBuffer[IndexBuffer[i]];
                int localIndex = 0;
                for (; localIndex < currentPatch.vertices.Count; localIndex++)
                {
                    if (currentPatch.vertices[localIndex] == v) break;
                }
                if (localIndex == currentPatch.vertices.Count) //pull new vertex
                {
                    if (currentPatch.vertices.Count == MAX_VERTICES)//current patch is full, start a new one
                    {
                        currentPatch.crop();
                        patches.Add(currentPatch);
                        currentPatch = new TrianglePatch();
                        i = (i / 3) * 3 - 1;
                        continue;
                    }
                    else
                        currentPatch.vertices.Add(v);
                }
                if (i % 3 == 0) currentTriangle.v1 = localIndex;
                if (i % 3 == 1) currentTriangle.v2 = localIndex;
                if (i % 3 == 2)
                {
                    currentTriangle.v3 = localIndex;
                    currentPatch.triangles.Add(currentTriangle);
                }
            }
            currentPatch.crop();
            patches.Add(currentPatch);
            Patches = patches.ToArray();
        }
    }
}
