using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SM64RAM
{

    public enum LEVEL_SCRIPT_COMMANDS : byte
    {
        LOAD_AND_JUMP = 0x00,
        LOAD_AND_JUMP_TERMINATE = 0x01,
        END_OF_LEVEL_LAYOUT = 0x02,
        DELAY_FRAMES = 0x03,
        UNKNOWN_04 = 0x04,
        GOTO = 0x05,
        BRANCH = 0x06,
        BRANCH_CONDITIONAL_RETURN = 0x07,
        BRANCH_RETURN = 0x0A,
        CONDITIONAL_BRANCH_LEVEL_ID = 0x0C,
        LOAD_ROM_TO_RAM = 0x10,
        CALL_FUNCTION_1 = 0x11,
        CALL_FUNCTION_2 = 0x12,
        SET_FUNCTION_ARGUMENT = 0x13,
        LOAD_ASM = 0x16,
        LOAD_UNCOMPRESSED = 0x17,
        LOAD_COMPRESSED = 0x18,
        MARIO_TITLE_SCREEN = 0x19,
        LOAD_COMPRESSED_2 = 0x1A,
        START_LOADING_SEQUENCE = 0x1B,
        BUILD_CAMERA_PRESETS = 0x1C,
        END_LOADING_SEQUENCE = 0x1D,
        BUILD_LEVEL_COLLISION = 0x1E,
        START_AREA = 0x1F,
        END_AREA = 0x20,
        SET_MODEL_ID_DISPLAYLIST = 0x21,
        SET_MODEL_ID_GEOLAYOUT = 0x22,
        PLACE_OBJECT = 0x24,
        LOAD_MARIO = 0x25,
        CONNECT_WARPS = 0x26,
        DEFINE_PAINTING_WARPS = 0x27,
        UNKNOWN_28 = 0x28,
        SET_INITIAL_POSITION = 0x2B,
        LOAD_COLLISION = 0x2E,
        UNKNOWN_2F = 0x2F,
        UNKNOWN_30 = 0x30,
        DEFINE_COLLISION_ENVIRONMENT = 0x31,
        _BLACKOUT_SCREEN_ = 0x34,
        SET_MUSIC = 0x36,
        SET_MUSIC_2 = 0x37,
        LOAD_MACRO_OBJECTS = 0x39,
        _FORCE_ = 0x3B,
        _GET_PUT_REMOTE_VALUES_ = 0x3C
    }

    public class LevelScriptReader
    {

        public delegate bool LevelScriptCommandExecutor(byte[] commandBytes);
        public static Dictionary<byte, List<LevelScriptCommandExecutor>> executors = new Dictionary<byte, List<LevelScriptCommandExecutor>>();

        static int cursor = 0;
        static int currentBankIndex = -1;
        static Stack<int> returnAddresses = new Stack<int>();
        static Stack<int> recordedJumps = new Stack<int>();
        static string log = "";
        public static int cursorPosition { get { 
            return currentBankIndex == -1 ? cursor : (currentBankIndex << 0x18) | cursor; } 
        }

        static LevelScriptReader()
        {
            foreach (byte value in Enum.GetValues(typeof(LEVEL_SCRIPT_COMMANDS)))
                executors[value] = new List<LevelScriptCommandExecutor>();
            executors[(byte)LEVEL_SCRIPT_COMMANDS.END_OF_LEVEL_LAYOUT].Add(JUMP_RETURN);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.BRANCH_RETURN].Add(JUMP_RETURN);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.BRANCH_CONDITIONAL_RETURN].Add(JUMP_RETURN);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.BRANCH].Add(BRANCH);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.CONDITIONAL_BRANCH_LEVEL_ID].Add(BRANCH);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_AND_JUMP].Add(LOAD_AND_JUMP);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_AND_JUMP_TERMINATE].Add(LOAD_AND_JUMP);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_UNCOMPRESSED].Add(LOAD_RAM_BANK);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_COMPRESSED].Add(LOAD_RAM_BANK);
            executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_COMPRESSED_2].Add(LOAD_RAM_BANK);
        }

        static void Log(string text)
        {
            log += text;
        }
        public static void SaveLog(string fileName)
        {
            System.IO.File.WriteAllText(fileName, log);
        }

        public static int[] GetCommands(int startPosition, int endPosition, params byte[] commands)
        {
            int cursor = startPosition;
            byte cmd, cmdLen;
            bool end = false;
            List<int> cmdList = new List<int>();
            while (!end)
            {
                cmd = EmulationState.instance.ROM[cursor];
                cmdLen = EmulationState.instance.ROM[cursor + 1];
                foreach (byte b in commands)
                    if (cmd == b)
                        cmdList.Add(cursor);
                cursor += cmdLen;
                if (cursor > endPosition) break;
                if (cmdLen < 4)
                {
                    EmulationState.messages.AppendMessage("Encountered invalid command 0x" + cmd.ToString("X") + " with length " + cmdLen.ToString() + " at 0x" + cursor.ToString("X") + ".", "Error");
                    return cmdList.ToArray();
                }
            }
            return cmdList.ToArray();
        }

        public static void ReadFrom(int startPosition, int end = -1)
        {
            cursor = startPosition;
            ReadAmount(EmulationState.instance.ROM, end == -1 ? -1 : end - startPosition);
        }

        static void ReadAmount(byte[] currentBank, int amount = -1)
        {
            foreach (int recordedJump in recordedJumps)
                if ((recordedJump & 0xFFFFFF) == cursor && EmulationState.instance.banks[recordedJump >> 0x18].value == currentBank)
                {
                    EmulationState.messages.AppendMessage("Infinite loop detected.", "Info");
                    return;
                }

            byte cmd, cmdLen;
            bool _continue = true;
            while (_continue)
            {
                if (amount >= 0 && amount-- <= 0)
                    break;
                for (int i = 0; i < EmulationState.instance.banks.Length; i++)
                    if (EmulationState.instance.banks[i] != null && EmulationState.instance.banks[i].value == currentBank)
                    {
                        currentBankIndex = i;
                        goto BankFound;
                    }
                currentBankIndex = -1;
                if (currentBank == EmulationState.instance.ROM) currentBankIndex = -2;
            BankFound:
                cmd = currentBank[cursor];
                cmdLen = currentBank[cursor + 1];
                byte[] commandBytes = new byte[cmdLen];
                Array.Copy(currentBank, cursor, commandBytes, 0, cmdLen);
                List<LevelScriptCommandExecutor> exec;
                if (executors.TryGetValue(cmd, out exec))
                    foreach (LevelScriptCommandExecutor ack in exec)
                        _continue &= ack(commandBytes);
                else
                {
                    EmulationState.messages.AppendMessage("Encountered unknown command 0x" + cmd.ToString("X") + " with length " + cmdLen.ToString() + " at 0x" + cursor.ToString("X") + " in bank 0x" + currentBankIndex.ToString("X") + ".", "Error");
                    return;
                }
                cursor += cmdLen;
                if (cmdLen < 4)
                {
                    EmulationState.messages.AppendMessage("Encountered invalid command 0x" + cmd.ToString("X") + " with length " + cmdLen.ToString() + " at 0x" + cursor.ToString("X") + ".", "Error");
                    return;
                }
            }
        }

        #region Command Implementations

        static bool LOAD_AND_JUMP(byte[] commandBytes)
        {
            int targetBank = commandBytes[3];
            int ROMStart = cvt.int32(commandBytes, 4);
            int ROMEnd = cvt.int32(commandBytes, 8);
            EmulationState.instance.LoadBank(targetBank, ROMStart, ROMEnd, false);
            //EmulationState.instance.banks[targetBank] = new EmulationState.RAMBank(targetBank, ROMStart, ROMEnd);
            if (currentBankIndex > -1)
                EmulationState.instance.banks[targetBank].ptrsRAM.Add((currentBankIndex << 0x18) | cursor);
            else if (currentBankIndex == -2)
                EmulationState.instance.banks[targetBank].ptrsROM.Add(cursor);

            EmulationState.RAMBank jumpTargetBank = EmulationState.instance.banks[commandBytes[0xC]];
            returnAddresses.Push(cursor);
            if (jumpTargetBank != null)
            {
                cursor = cvt.int32(commandBytes, 0xC) & 0xFFFFFF;
                ReadAmount(jumpTargetBank.value);
            }
            else
                Log("Skipped jump to RAM bank 0x" + (commandBytes[0xC]).ToString("X") + ".");
            cursor = returnAddresses.Pop();
            return commandBytes[0] == 0x00;
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

        static bool LOAD_RAM_BANK(byte[] commandBytes)
        {
            int targetBank = commandBytes[3];
            int ROMStart = cvt.int32(commandBytes, 4);
            int ROMEnd = cvt.int32(commandBytes, 8);
            EmulationState.instance.LoadBank(targetBank, ROMStart, ROMEnd, commandBytes[0] != (byte)LEVEL_SCRIPT_COMMANDS.LOAD_UNCOMPRESSED);
            //EmulationState.instance.banks[targetBank] = new EmulationState.RAMBank(targetBank, ROMStart, ROMEnd, commandBytes[0] != (byte)LEVEL_SCRIPT_COMMANDS.LOAD_UNCOMPRESSED);
            return true;
        }

        static bool JUMP_RETURN(byte[] commandBytes) { return false; }

        #endregion

    }
}
