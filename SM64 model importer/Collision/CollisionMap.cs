using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SM64RAM;

namespace SM64_model_importer
{
    public class CollisionTriangle
    {
        public short i1, i2, i3;
        public short extraInformation;
        public CollisionTriangle(short i1, short i2, short i3)
        {
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
        }
    }

    public class CollisionPatch
    {
        public string name;
        public short type;
        public List<CollisionTriangle> triangles = new List<CollisionTriangle>();
        public CollisionPatch(string name)
        {
            this.name = name;
        }
    }

    public class Collision
    {
        public enum PatchMode
        {
            ByObject = 0,
            ByMaterial = 1
        }

        public static byte[] objectPresets0x43;

        public Vector3[] allVertices = new Vector3[0];
        public List<CollisionPatch> patches = new List<CollisionPatch>();

        private static bool hasExtraInformation(short cmdByte)
        {
            cmdByte -= 4;
            if (cmdByte == 0 || cmdByte == 0xA || cmdByte == 0x20 || cmdByte == 0x21 || cmdByte == 0x23 || cmdByte == 0x28 || cmdByte == 0x29)
                return true;
            return false;
        }

        public void Load(byte[] currentBank, int offset, Level level, Area area)
        {
            int cursor = offset;
            while (true)
            {
                byte command = currentBank[cursor + 1];
                cursor += 2;

                if (command < 0x40 || command >= 0x65)
                {
                    bool extraInformation = hasExtraInformation(command);
                    int numTriangles = cvt.int16(currentBank, cursor);
                    cursor += 2;
                    CollisionPatch currentPatch = new CollisionPatch("Patch " + offset.ToString("X"));
                    currentPatch.type = command;
                    patches.Add(currentPatch);

                    for (int i = 0; i < numTriangles; i++)
                    {
                        CollisionTriangle t;
                        currentPatch.triangles.Add(t = new CollisionTriangle(cvt.int16(currentBank, cursor), cvt.int16(currentBank, cursor + 2), cvt.int16(currentBank, cursor + 4)));
                        cursor += 6;
                        if (extraInformation)
                        {
                            t.extraInformation = cvt.int16(currentBank, cursor);
                            cursor += 2;
                        }
                    }
                    continue;
                }

                if (command == 0x40)
                {
                    allVertices = new Vector3[cvt.int16(currentBank, cursor)];
                    cursor += 2;
                    for (int i = 0; i < allVertices.Length; i++)
                    {
                        allVertices[i] = new Vector3(-cvt.int16(currentBank, cursor), cvt.int16(currentBank, cursor + 2), cvt.int16(currentBank, cursor + 4));
                        cursor += 6;
                    }
                    continue;
                }

                if (command == 0x43) //Special objects
                {
                    int numObjects = cvt.int16(currentBank, cursor);
                    cursor += 2;
                    for (int i = 0; i < numObjects; i++)
                    {
                        int objectPreset = currentBank[cursor + 1];
                        int currentPreset = 0;
                        while (objectPresets0x43[currentPreset] != objectPreset)
                            currentPreset += 8;

                        Vector3 position = new Vector3(-cvt.int16(currentBank, cursor + 2), cvt.int16(currentBank, cursor + 4), cvt.int16(currentBank, cursor + 6));
                        cursor += 8;
                        float yRot = 0;
                        int behaviour = cvt.int32(objectPresets0x43, currentPreset + 4);
                        int mID = objectPresets0x43[currentPreset + 3];
                        int bParams = 0;

                        switch (objectPresets0x43[currentPreset + 1])
                        {
                            case 0: break;
                            case 1: yRot = (short)(currentBank[cursor + 1] << 0x8); cursor += 2; break;
                            case 2: yRot = cvt.int16(currentBank, cursor); bParams = cvt.int16(currentBank, cursor + 2); cursor += 4; break;
                            case 3: yRot = cvt.int16(currentBank, cursor); bParams = cvt.int16(currentBank, cursor + 2); cursor += 4; break;
                            case 4: bParams = cvt.int16(currentBank, cursor); cursor += 2; break;
                        }
                        yRot = ((float)yRot / ushort.MaxValue) * 360;
                        if (level != null && area != null)
                        {
                            Object newObject = new Object(position, new Vector3(0, yRot, 0), behaviour, mID, 0x1F, bParams);
                            area.AddObject(newObject, Area.ObjectStorageType.Special);
                        }
                    }
                    continue;
                }
                if (command == 0x44) //water box
                {
                    int numObjects = cvt.int16(currentBank, cursor);
                    cursor += 2;
                    for (int i = 0; i < numObjects; i++)
                    {
                        //WaterBox asfas = new WaterBox(cvt.int16(currentBank, cursor + 2), cvt.int16(currentBank, cursor + 4), cvt.int16(currentBank, cursor + 6), cvt.int16(currentBank, cursor + 8), cvt.int16(currentBank, cursor + 10));
                        //asfas.ID = cvt.int16(currentBank, cursor);
                        cursor += 12;
                    }
                }

                if (command == 0x41)
                    continue;

                if (command == 0x42)
                    break;
            }
        }

        public int GetLength()
        {
            int length = allVertices.Length * 6 + 8 + patches.Count * 4;
            foreach (CollisionPatch p in patches)
                length += p.triangles.Count * (hasExtraInformation(p.type) ? 8 : 6);
            return (length + 3) / 4 * 4;
        }

        public void Write(ref byte[] stream, int cursor)
        {
            int end = cursor + GetLength();

            cvt.writeInt16(stream, cursor, 0x40);
            cvt.writeInt16(stream, cursor + 2, (short)allVertices.Length);
            cursor += 4;
            foreach (Vector3 vertex in allVertices)
            {
                cvt.writeInt16(stream, cursor, (short)-vertex.X);
                cvt.writeInt16(stream, cursor + 2, (short)vertex.Y);
                cvt.writeInt16(stream, cursor + 4, (short)vertex.Z);
                cursor += 6;
            }

            foreach (CollisionPatch p in patches)
            {
                cvt.writeInt16(stream, cursor, (short)p.type);
                cvt.writeInt16(stream, cursor + 2, (short)p.triangles.Count);
                cursor += 4;
                bool extraInformation = hasExtraInformation(p.type);
                foreach (CollisionTriangle t in p.triangles)
                {
                    cvt.writeInt16(stream, cursor, t.i1);
                    cvt.writeInt16(stream, cursor + 2, t.i2);
                    cvt.writeInt16(stream, cursor + 4, t.i3);
                    cvt.writeInt16(stream, cursor + 6, t.extraInformation);
                    cursor += extraInformation ? 8 : 6;
                }
            }
            cvt.writeInt16(stream, cursor, 0x41);
            cvt.writeInt16(stream, cursor + 2, 0x42);
            cursor = end;
        }

        public void Clear()
        {
            patches.Clear();
            allVertices = new Vector3[0];
        }

        public void Import(string fileName, PatchMode mode)
        {
            Clear();

            string[] fileNameSplit = fileName.Split(new char[] { '/', '\\' });
            string fileRelativePath = fileName.Remove(fileName.Length - (fileNameSplit[fileNameSplit.Length - 1].Length), fileNameSplit[fileNameSplit.Length - 1].Length);

            int minIndex = allVertices.Length;
            List<Vector3> positions = new List<Vector3>();
            CollisionPatch currentPatch = new CollisionPatch("<Undefined>");

            StreamReader rd = new StreamReader(fileName);

            while (!rd.EndOfStream)
            {
                string line = rd.ReadLine();
                string[] split = line.Split(' ');
                if (split[0] == "o") //Object
                {
                    if (mode == PatchMode.ByObject)
                    {
                        if (currentPatch.triangles.Count > 0)
                            patches.Add(currentPatch);
                        currentPatch = new CollisionPatch(split[1]);
                    }
                }
                else if (split[0] == "v")
                {
                    Vector3 v = cvt.ParseVector3(split, 1);
                    v.X *= -1;
                    positions.Add(v * Settings.importScale);
                }
                else if (split[0] == "usemtl")
                {
                    if (mode == PatchMode.ByMaterial)
                    {
                        if (currentPatch.triangles.Count > 0)
                        {
                            foreach (CollisionPatch patch in patches)
                                if (patch.name == currentPatch.name)
                                    goto skipMakePatch;
                            patches.Add(currentPatch);
                        skipMakePatch: ;
                        }
                        foreach (CollisionPatch patch in patches)
                            if (patch.name == split[1])
                            {
                                currentPatch = patch;
                                goto skipNewPatch;
                            }
                        currentPatch = new CollisionPatch(split[1]);
                    skipNewPatch: ;
                    }
                }
                else if (split[0] == "f") // Face
                {
                    short i0 = 0, ix = 0;
                    for (int i = 1; i < split.Length; i++)
                    {
                        short index = 0;
                        string[] vertexIndices = split[i].Split('/');
                        short.TryParse(vertexIndices[0], out index);
                        index -= 1;

                        if (i == 1) i0 = index;
                        if (i >= 3)
                        {
                            CollisionTriangle t;
                            currentPatch.triangles.Add(t = new CollisionTriangle((short)(i0 + minIndex), (short)(ix + minIndex), (short)(index + minIndex)));
                        }
                        ix = index;
                    }
                }
            }
            rd.BaseStream.Close();
            Array.Resize(ref allVertices, allVertices.Length + positions.Count);
            Array.Copy(positions.ToArray(), 0, allVertices, allVertices.Length - positions.Count, positions.Count);

            if (currentPatch.triangles.Count > 0)
            {
                if (mode == PatchMode.ByObject)
                    goto makePatch;
                foreach (CollisionPatch patch in patches)
                    if (patch.name == currentPatch.name)
                        goto skipMakePatch;
            makePatch:
                patches.Add(currentPatch);
            skipMakePatch: ;
            }
            Optimize();
        }

        public void Optimize()
        {
            int numVertsBefore = allVertices.Length;
            int n = 0;
            for (short i = 0; i < allVertices.Length - n; i++)
                for (short k = (short)(i + 1); k < allVertices.Length - n; k++)
                    if (allVertices[k] == allVertices[i])
                    {
                        foreach (CollisionPatch patch in patches)
                            foreach (CollisionTriangle t in patch.triangles)
                            {
                                if (t.i1 == k) t.i1 = i;
                                else if (t.i1 > k) t.i1 -= 1;
                                if (t.i2 == k) t.i2 = i;
                                else if (t.i2 > k) t.i2 -= 1;
                                if (t.i3 == k) t.i3 = i;
                                else if (t.i3 > k) t.i3 -= 1;
                            }
                        n++;
                        for (int del = k; del < allVertices.Length - n; del++)
                            allVertices[del] = allVertices[del + 1];
                        k--;
                    }
            Array.Resize(ref allVertices, allVertices.Length - n);
        }
    }
}
