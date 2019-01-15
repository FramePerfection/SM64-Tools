using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;

namespace SM64Renderer
{
    public class DisplayListReader
    {
        delegate void DisplayListCommand(DisplayListReader dl, long command);
        static Dictionary<byte, DisplayListCommand> commands = new Dictionary<byte, DisplayListCommand>();
        static DisplayListReader()
        {
            commands[(byte)F3D.ENDDL] = (DisplayListReader dl, long command) =>
            {
                if (dl.callStack.Count > 0)
                    dl.cursor = dl.callStack.Pop();
                else
                    dl.terminated = true;
            };
            commands[(byte)F3D.DL] = (DisplayListReader dl, long command) =>
            {
                if ((command & 0x0001000000000000) == 0)
                    dl.callStack.Push(dl.cursor);
                dl.cursor = (int)((command & 0xFFFFFFFF) - 8);
            };
            commands[(byte)F3D.VTX] = (DisplayListReader dl, long command) => dl.output.LoadVertices(command);
            commands[(byte)F3D.TRI1] = (DisplayListReader dl, long command) => dl.output.CreateTriangle(command);
            commands[(byte)GBI.G_SETTILE] = (DisplayListReader dl, long command) => dl.output.SetTile(command);
            commands[(byte)GBI.G_SETTILESIZE] = (DisplayListReader dl, long command) => dl.output.SetTileSize(command);
            commands[(byte)GBI.G_LOADBLOCK] = (DisplayListReader dl, long command) => dl.output.LoadBlock(command);
            commands[(byte)GBI.G_SETTIMG] = (DisplayListReader dl, long command) => dl.output.SetTexture(command);
        }

        Stack<int> callStack;
        int cursor = 0;
        bool terminated = false;
        DisplayList output;

        public static DisplayList Read(int segmentedPointer, EmulationState state, TextureManager texMan)
        {
            StringBuilder log = new StringBuilder();
            DisplayListReader reader = new DisplayListReader();
            reader.output = new DisplayList(texMan);
            int bank = segmentedPointer >> 0x18;
            int offset = segmentedPointer & 0xFFFFFF;
            if (!state.AssertRead(segmentedPointer, 8)) return null;
            EmulationState.RAMBank b = state.banks[bank];
            reader.cursor = segmentedPointer;
            reader.callStack = new Stack<int>();
            long currentCommand;
            do
            {
                b = state.banks[reader.cursor >> 0x18];
                offset = reader.cursor & 0xFFFFFF;
                currentCommand = ((long)cvt.uint32(b.value, offset) << 0x20) | (long)cvt.uint32(b.value, offset + 4);
                byte commandByte = b.value[offset];
                DisplayListCommand cmd;
                if (commands.TryGetValue(commandByte, out cmd))
                    cmd(reader, currentCommand);
                else
                    log.Append("Skipped command " + Enum.GetName(typeof(GBI), commandByte) + " - " + currentCommand.ToString("X16") + "\n");

                reader.cursor += 8;
            } while (!reader.terminated);

            System.Diagnostics.Debug.WriteLine(log.ToString());
            return reader.output;
        }
    }
}
