using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SM64RAM
{

    public enum GEO_LAYOUT_COMMANDS : byte
    {
        BRANCH_AND_STORE = 0x00,
        GEO_LAYOUT_END = 0x01,
        BRANCH = 0x02,
        BRANCH_RETURN = 0x03,
        START_HIERARCHY_NODE = 0x04,
        END_HIERARCHY_NODE = 0x05,
        SET_VIEWPORT = 0x08,
        _UNKNONW_SCALE_ = 0x09,
        _SET_PROJECTION_ = 0x0A,
        START_PARAMETERLESS = 0x0B,
        SET_DEPTHBUFFER = 0x0C,
        SET_RENDER_RANGE = 0x0D,
        _SET_ANIMATION_ = 0x0E,
        SET_CAMEREA_PRESET = 0x0F,
        TRANSLATE_AND_ROTATE = 0x10,
        UNKNOWN_1 = 0x11,
        UNKNOWN_4 = 0x12,
        UNKNOWN_5 = 0x13,
        TRANSFORMED_DISPLAYLIST = 0x13,
        BILLBOARD = 0x14,
        DISPLAYLIST = 0x15,
        SHADOW = 0x16,
        _ADD_MARIO_ = 0x17,
        ENVIRONMENT_EFFECT = 0x18,
        SET_BACKGROUND_FUNCTION = 0x19,
        UNKNOWN_6 = 0x1C,
        SCALE_POLYGONS = 0x1D,
        DRAWING_DISTANCE = 0x20
    }

    public class GeoLayoutReader
    {

        public delegate bool GeoLayoutCommandExecutor(byte[] commandBytes);
        public static Dictionary<byte, List<GeoLayoutCommandExecutor>> executors = new Dictionary<byte, List<GeoLayoutCommandExecutor>>();
        static Dictionary<byte, byte> commandLength = new Dictionary<byte, byte>();

        static int cursor;
        static Stack<int> returnAddresses = new Stack<int>();
        static string log = "";

        static GeoLayoutReader()
        {
            foreach (byte value in Enum.GetValues(typeof(GEO_LAYOUT_COMMANDS)))
                executors[value] = new List<GeoLayoutCommandExecutor>();
            commandLength[(byte)GEO_LAYOUT_COMMANDS.BRANCH_AND_STORE] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.GEO_LAYOUT_END] = 4;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.BRANCH] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.BRANCH_RETURN] = 4;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.START_HIERARCHY_NODE] = 4;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.END_HIERARCHY_NODE] = 4;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.SET_VIEWPORT] = 12;
            commandLength[(byte)GEO_LAYOUT_COMMANDS._UNKNONW_SCALE_] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS._SET_PROJECTION_] = 12;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.START_PARAMETERLESS] = 4;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.SET_DEPTHBUFFER] = 4;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.SET_RENDER_RANGE] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS._SET_ANIMATION_] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.SET_CAMEREA_PRESET] = 20;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.TRANSLATE_AND_ROTATE] = 16;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.UNKNOWN_1] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.UNKNOWN_4] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.UNKNOWN_5] = 16;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.TRANSFORMED_DISPLAYLIST] = 12;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.BILLBOARD] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.DISPLAYLIST] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.SHADOW] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS._ADD_MARIO_] = 4;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.ENVIRONMENT_EFFECT] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.SET_BACKGROUND_FUNCTION] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.UNKNOWN_6] = 12;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.SCALE_POLYGONS] = 8;
            commandLength[(byte)GEO_LAYOUT_COMMANDS.DRAWING_DISTANCE] = 4;

            executors[(byte)GEO_LAYOUT_COMMANDS.GEO_LAYOUT_END].Add(JUMP_RETURN);
            executors[(byte)GEO_LAYOUT_COMMANDS.BRANCH].Add(BRANCH);
            executors[(byte)GEO_LAYOUT_COMMANDS.BRANCH_RETURN].Add(JUMP_RETURN);
        }

        static void Log(string text)
        {
            log += text;
        }

        public static void SaveLog(string fileName)
        {
            System.IO.File.WriteAllText(fileName, log);
        }


        public static void ReadFromROM(int startPosition)
        {
            cursor = startPosition;
            ReadAmount(EmulationState.instance.ROM, startPosition);
        }

        public static void ReadFromSegmented(int segmentedAddress)
        {
            if (!EmulationState.instance.AssertRead(segmentedAddress, 4)) return;
            cursor = segmentedAddress & 0xFFFFFF;
            try
            {
                ReadAmount(EmulationState.instance.banks[segmentedAddress >> 0x18].value);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        static void ReadAmount(byte[] currentBank, int amount = -1)
        {
            byte cmd = 0, cmdLen;
            bool _continue = true;
            while (_continue)
            {
                if (amount >= 0 && amount-- <= 0 || cursor >= currentBank.Length)
                    break;
                    cmd = currentBank[cursor];
                if (!commandLength.TryGetValue(cmd, out cmdLen))
                {
                    EmulationState.messages.AppendMessage("Encountered unknown command 0x" + cmd.ToString("X") + " at 0x" + cursor.ToString("X") + " and cannot derive length!", "Error");
                    return;
                }
                if (cmd == 0xA && currentBank[cursor + 1] != 0) cmdLen += 4;
                byte[] commandBytes = new byte[cmdLen];
                Array.Copy(currentBank, cursor, commandBytes, 0, cmdLen);
                List<GeoLayoutCommandExecutor> exec;
                if (executors.TryGetValue(cmd, out exec))
                    foreach (GeoLayoutCommandExecutor ack in exec)
                    {
                        _continue &= ack(commandBytes);
                        if (!_continue && cmd != (byte)GEO_LAYOUT_COMMANDS.GEO_LAYOUT_END && cmd != (byte)GEO_LAYOUT_COMMANDS.BRANCH_RETURN)
                            MessageBox.Show("Command executor " + ack.Method.Name + " caused premature exiting of the geo layout.");
                    }
                else
                {
                    MessageBox.Show("Encountered unknown command 0x" + cmd.ToString("X") + " with length " + cmdLen.ToString() + " at 0x" + cursor.ToString("X") + "."
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cursor += cmdLen;
                if (cmdLen < 4)
                {
                    MessageBox.Show("Encountered invalid command 0x" + cmd.ToString("X") + " with length " + cmdLen.ToString() + " at 0x" + cursor.ToString("X") + "."
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        static bool BRANCH(byte[] commandBytes)
        {
            uint target = cvt.uint32(commandBytes, 4);
            EmulationState.RAMBank jumpTargetBank = EmulationState.instance.banks[target >> 0x18];
            returnAddresses.Push(cursor);
            if (jumpTargetBank != null)
            {
                cursor = (int)(target & 0xFFFFFF);
                ReadAmount(EmulationState.instance.banks[target >> 0x18].value);
            }
            else
                Log("Skipped jump to RAM bank 0x" + (target >> 0x18).ToString("X") + ".");
            cursor = returnAddresses.Pop();
            return commandBytes[3] == 0;
        }

        static bool JUMP_RETURN(byte[] commandBytes) { return false; }
    }
}
