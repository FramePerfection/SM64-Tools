using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;
using SM64Renderer;

namespace SM64LevelEditor
{
    public class Level
    {
        public static DeviceWrapper renderDevice;
        public static Renderer renderer;
        public static Dictionary<byte, GeoLayout> modelIDs = new Dictionary<byte, GeoLayout>();

        static int currentArea = -1;
        static Level current;


        public List<Alias<int>> behaviourAlias = new List<Alias<int>>();
        public List<Alias<byte>> modelIDAlias = new List<Alias<byte>>();
        public TextureManager textureManager;

        public int segmentedPointer { get; private set; }
        public Area[] areas = new Area[0];
        Dictionary<byte, int> levelGeos = new Dictionary<byte, int>();
        List<byte[]> loadedBanks = new List<byte[]>();

        public static void InitCommands()
        {
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_AND_JUMP].Add(LOAD_AND_JUMP);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_UNCOMPRESSED].Add(LOAD_BANK);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_COMPRESSED].Add(LOAD_BANK);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_COMPRESSED_2].Add(LOAD_BANK);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.START_AREA].Add(START_AREA);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_COLLISION].Add(LOAD_COLLISION);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.CONNECT_WARPS].Add(CONNECT_WARPS);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.END_AREA].Add(END_AREA);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.PLACE_OBJECT].Add(PLACE_OBJECT);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.SET_MODEL_ID_DISPLAYLIST].Add(SET_MODEL_ID);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.SET_MODEL_ID_GEOLAYOUT].Add(SET_MODEL_ID);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.BRANCH].Add((byte[] stuff) =>
            {
                return true;
            });
        }

        public static Level LoadLevelROM(int ROMstart)
        {
            current = new Level();
            GeoLayout.textureManager = current.textureManager;
            LevelScriptReader.ReadFrom(ROMstart);
            Level output = current;
            current = null;
            return output;
        }


        #region Level Script Implementations

        static bool LOAD_AND_JUMP(byte[] commandBytes)
        {
            if (current != null)
            current.segmentedPointer = cvt.int32(commandBytes, 0xC);
            return true;
        }

        static bool LOAD_BANK(byte[] commandBytes)
        {
            if (current != null)
                current.loadedBanks.Add(commandBytes);
            return true;
        }

        static bool START_AREA(byte[] commandBytes)
        {
            if (current == null) return false;

            int index = commandBytes[2];
            if (index > current.areas.Length)
                Array.Resize(ref current.areas, index);
            current.areas[index - 1] = new Area(current, cvt.int32(commandBytes, 4));
            currentArea = index - 1;
            return true;
        }

        static bool LOAD_COLLISION(byte[] commandBytes)
        {
            if (current == null) return false;
            if (currentArea == -1) return false;
            current.areas[currentArea].SetCollision(cvt.int32(commandBytes, 4));
            return true;
        }

        static bool CONNECT_WARPS(byte[] commandBytes)
        {
            if (current == null) return false;
            if (currentArea == -1) return false;
            current.areas[currentArea].warps.Add(new Warp(commandBytes[2], commandBytes[3], commandBytes[4], commandBytes[5]));
            return true;
        }

        static bool END_AREA(byte[] commandBytes)
        {
            if (current == null || currentArea == -1) return false;
            currentArea = -1;
            return true;
        }

        static bool SET_MODEL_ID(byte[] commandBytes)
        {
            if (commandBytes[0] == 0x21) return true;
            int address = cvt.int32(commandBytes, 4);
            GeoLayout lol = GeoLayout.LoadSegmented(address);
            if (lol != null)
                Level.modelIDs[commandBytes[3]] = lol;
            if (current != null)
                current.levelGeos[commandBytes[3]] = address;
            return true;
        }

        static bool PLACE_OBJECT(byte[] commandBytes)
        {
            Object newObject = new Object(current, current.areas[currentArea]);
            newObject.acts = commandBytes[2];
            newObject.SetModelID(commandBytes[3]);
            newObject.positionX = cvt.int16(commandBytes, 4);
            newObject.positionY = cvt.int16(commandBytes, 6);
            newObject.positionZ = cvt.int16(commandBytes, 8);
            newObject.rotationX = cvt.int16(commandBytes, 10);
            newObject.rotationY = cvt.int16(commandBytes, 12);
            newObject.rotationZ = cvt.int16(commandBytes, 14);
            newObject.bParam = cvt.int32(commandBytes, 16);
            newObject.behaviourScript = cvt.int32(commandBytes, 20);
            current.areas[currentArea].AddObject(newObject);
            return true;
        }

        #endregion

        public Level()
        {
            if (renderDevice == null)
                throw new Exception("Render Device must be set!");
            textureManager = new TextureManager(EmulationState.instance, renderDevice);
        }

        public void SetAliasFile(string behaviourFile, string modelIDFile)
        {
            behaviourAlias = Alias<int>.LoadFile(behaviourFile, (string str) => { int output = 0; if (cvt.ParseIntHex(str.Substring(2), out output)) return output; return -1; }, (int v) => "0x" + v.ToString("X8"));
            modelIDAlias = Alias<byte>.LoadFile(modelIDFile, (string str) => { int output = 0; if (cvt.ParseIntHex(str.Substring(2), out output) && output <= byte.MaxValue) return (byte)output; return 0xFF; }, (byte v) => "0x" + v.ToString("X2"));
            ToolBox.instance.UpdateAlias();
        }

        public void MakeVisible(int area)
        {
            renderer.ClearLayers();
            areas[area].MakeVisible();
        }

        public void CreateCommands(int segmentedPointer)
        {
            if (!EmulationState.instance.AssertWrite(segmentedPointer, 4)) return;
            byte[] bank = EmulationState.instance.banks[segmentedPointer >> 0x18].value;
            System.Windows.Forms.MessageBox.Show(EmulationState.instance.banks[segmentedPointer >> 0x18].ROMStart.ToString("X8"));
            int cursor = segmentedPointer & 0xFFFFFF;
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.START_LOADING_SEQUENCE);
            foreach (byte[] loadedBank in loadedBanks)
            {
                Array.Copy(loadedBank, 0, bank, cursor, loadedBank.Length);
                cursor += loadedBank.Length;
            }
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.END_LOADING_SEQUENCE);
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.LOAD_MARIO, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x13, 0x00, 0x2E, 0xC0);
            foreach (KeyValuePair<byte, int> acken in levelGeos)
                WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.SET_MODEL_ID_GEOLAYOUT, acken.Key, acken.Value);
            for (int i = 0; i < areas.Length; i++)
            {
                if (areas[i] == null) continue;
                WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.START_AREA, (short)((i + 1) << 0x8), areas[i].geometry.segmentedPointer);
                WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.LOAD_COLLISION, 0, areas[i].collisionPointer);
                foreach (Object obj in areas[i])
                    WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.PLACE_OBJECT, obj.GetParameterBytes());
                foreach (Warp warp in areas[i].warps)
                    WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.CONNECT_WARPS, warp.sourceID, warp.destinationLevel, warp.destinationArea, warp.destinationID);
                WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.END_AREA);
            }

            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.BUILD_LEVEL_COLLISION);
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.CALL_FUNCTION_1, 0x00, 0x00, 0x80, 0x24, 0xBC, 0xD8);
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.CALL_FUNCTION_2, 0x00, 0x01, 0x80, 0x24, 0xBC, 0xD8);

            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.END_OF_LEVEL_LAYOUT);
        }

        void WriteCommand(ref int cursor, byte[] targetBank, LEVEL_SCRIPT_COMMANDS command, params byte[] parameters)
        {
            int alignedLength = ((parameters.Length + 5) / 4) * 4;
            targetBank[cursor] = (byte)command;
            targetBank[cursor + 1] = (byte)alignedLength;
            Array.Copy(parameters, 0, targetBank, cursor + 2, parameters.Length);
            int cursorEnd = cursor + parameters.Length + 2;
            for (int i = cursorEnd; i < cursor + alignedLength; i++)
                targetBank[i] = 0;
            cursor += alignedLength;
        }

        void WriteCommandWords(ref int cursor, byte[] targetBank, LEVEL_SCRIPT_COMMANDS command, short paramsHigh, params int[] moreParams)
        {
            int alignedLength = 4 * (moreParams.Length + 1);
            targetBank[cursor] = (byte)command;
            targetBank[cursor + 1] = (byte)alignedLength;
            targetBank[cursor + 2] = (byte)(paramsHigh >> 0x8);
            targetBank[cursor + 3] = (byte)(paramsHigh & 0xFF);
            for (int i = 0; i < moreParams.Length; i++)
                cvt.writeInt32(targetBank, cursor + 4 * (i + 1), moreParams[i]);
            cursor += alignedLength;
        }
    }
}
