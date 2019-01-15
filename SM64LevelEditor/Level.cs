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
        List<int> levelGeoJumps = new List<int>();
        public List<BankDescription> loadedBanks = new List<BankDescription>();
        bool recordingLevelGeos = true, recordingGeoJumps = false;

        Vector3 initialPosition;
        int initialAngle;
        byte initialUnknown;
        byte collisionEnvironment;

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
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.SET_INITIAL_POSITION].Add(SET_INITIAL_POSITION);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.DEFINE_COLLISION_ENVIRONMENT].Add(DEFINE_COLLISION_ENVIRONMET);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_MARIO].Add((byte[] stuff) => current.recordingGeoJumps = true);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.BRANCH].Add((byte[] stuff) =>
            {
                if (current != null)
                {
                    if (current.recordingGeoJumps)
                        current.levelGeoJumps.Add(cvt.int32(stuff, 4));
                    current.recordingLevelGeos = false;
                }
                return true;
            });
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.BRANCH_RETURN].Add((byte[] stuff) =>
            {
                if (current != null)
                    current.recordingLevelGeos = true;
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
                current.loadedBanks.Add(BankDescription.Parse(commandBytes));
            return true;
        }

        static bool START_AREA(byte[] commandBytes)
        {
            if (current == null) return false;
            current.recordingGeoJumps = false;

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
            try
            {
                if (commandBytes[0] == 0x21) return true;
                int address = cvt.int32(commandBytes, 4);
                GeoLayout lol = GeoLayout.LoadSegmented(address);
                if (lol != null)
                    Level.modelIDs[commandBytes[3]] = lol;
                if (current != null && current.recordingLevelGeos)
                    current.levelGeos[commandBytes[3]] = address;
                return true;
            }
            catch (Exception ex)
            {
                ;
            }
            return false;
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

        static bool SET_INITIAL_POSITION(byte[] commandBytes)
        {
            current.initialUnknown = commandBytes[2];
            current.initialAngle = cvt.int16(commandBytes, 4);
            current.initialPosition = new Vector3(cvt.int16(commandBytes, 6), cvt.int16(commandBytes, 8), cvt.int16(commandBytes, 10));
            return true;
        }

        static bool DEFINE_COLLISION_ENVIRONMET(byte[] commandBytes)
        {
            current.collisionEnvironment = commandBytes[3];
            return true;
        }

        static bool SET_MUSIC(byte[] commandBytes)
        {
            current.areas[currentArea].musicSequence = cvt.int16(commandBytes, 4);
            return true;
        }

        #endregion

        public Level()
        {
            if (renderDevice == null)
                throw new Exception("Render Device must be set!");
            textureManager = new TextureManager(EmulationState.instance, renderDevice);
        }

        public void SetAliasFile(List<Alias<int>> behaviourAlias, List<Alias<byte>> modelIDAlias)
        {
            this.behaviourAlias = behaviourAlias;
            this.modelIDAlias = modelIDAlias;
            ToolBox.instance.UpdateAlias();
        }

        public void MakeVisible(int area)
        {
            renderer.ClearLayers();
            areas[area].MakeVisible();
        }

        public void ReadJump(byte[] jumpCommand)
        {
            recordingLevelGeos = false;
            int jumpAddress = cvt.int32(jumpCommand, 4);
            levelGeoJumps.Add(jumpAddress);
            current = this;
            LevelScriptReader.ReadFromSegmented(jumpAddress);
            current = null;
            recordingLevelGeos = true;
        }

        public void AddLevelGeo(byte id, GeoLayout layout)
        {
            levelGeos[id] = layout.segmentedPointer;
            Level.modelIDs[id] = layout;
        }

        public void CleanLevelGeos()
        {
            Dictionary<byte, int> newGeo = new Dictionary<byte, int>(levelGeos);
            levelGeos.Clear();

            current = this;
            foreach (int jumpAddress in levelGeoJumps)
                LevelScriptReader.ReadFromSegmented(jumpAddress);
            current = null;

            foreach (var jumpGeoLayout in levelGeos)
                newGeo.Remove(jumpGeoLayout.Key);
            levelGeos.Clear();

            foreach (var layout in newGeo)
            {
                try
                {
                    GeoLayout.LoadSegmented(layout.Value);
                    levelGeos[layout.Key] = layout.Value;
                }
                catch
                {
                    //Geo layout could not be loaded, thus should not be added to the levels geo layout list.
                }
            }
            levelGeos = newGeo;
            foreach (Area area in areas)
                foreach (Object obj in area)
                {
                    byte id = obj.model_ID;
                    obj.SetModelID(0);
                    obj.SetModelID(id);
                }
        }

        public void CreateCommands(int segmentedPointer)
        {
            if (!EmulationState.instance.AssertWrite(segmentedPointer, 4)) return;
            byte[] bank = EmulationState.instance.banks[segmentedPointer >> 0x18].value;
            int cursor = segmentedPointer & 0xFFFFFF;
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.START_LOADING_SEQUENCE);
            foreach (BankDescription loadedBank in loadedBanks)
            {
                WriteCommand(ref cursor, bank, loadedBank.compressed ? LEVEL_SCRIPT_COMMANDS.LOAD_COMPRESSED : LEVEL_SCRIPT_COMMANDS.LOAD_UNCOMPRESSED, loadedBank.arg, loadedBank.ID,
                                (byte)(loadedBank.ROM_Start >> 0x18), (byte)(loadedBank.ROM_Start >> 0x10), (byte)(loadedBank.ROM_Start >> 0x8), (byte)(loadedBank.ROM_Start),
                                (byte)(loadedBank.ROM_End >> 0x18), (byte)(loadedBank.ROM_End >> 0x10), (byte)(loadedBank.ROM_End >> 0x8), (byte)(loadedBank.ROM_End));
            }
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.END_LOADING_SEQUENCE);
            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.LOAD_MARIO, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x13, 0x00, 0x2E, 0xC0);
            foreach (var geo in levelGeos)
                WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.SET_MODEL_ID_GEOLAYOUT, geo.Key, geo.Value);
            foreach (int geoJump in levelGeoJumps)
                WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.BRANCH, 0, geoJump);

            //foreach (KeyValuePair<byte, int> acken in levelGeos)
            //    WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.SET_MODEL_ID_GEOLAYOUT, acken.Key, acken.Value);
            for (int i = 0; i < areas.Length; i++)
            {
                if (areas[i] == null) continue;
                WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.START_AREA, (short)((i + 1) << 0x8), areas[i].geometry.segmentedPointer);
                WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.LOAD_COLLISION, 0, areas[i].collisionPointer);
                foreach (Object obj in areas[i])
                    WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.PLACE_OBJECT, obj.GetParameterBytes());
                foreach (Warp warp in areas[i].warps)
                    WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.CONNECT_WARPS, warp.sourceID, warp.destinationLevel, warp.destinationArea, warp.destinationID);
                if (areas[i].musicSequence != -1)
                    WriteCommandWords(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.SET_MUSIC, 0, areas[i].musicSequence << 0x10);
                WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.END_AREA);
            }

            WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.BUILD_LEVEL_COLLISION);
            if (initialUnknown == 1) //Seems to determine wether this level can use its initial position. Not sure about this.
            {
                short x = (short)initialPosition.X, y = (short)initialPosition.Y, z = (short)initialPosition.Z;
                WriteCommand(ref cursor, bank, LEVEL_SCRIPT_COMMANDS.SET_INITIAL_POSITION, initialUnknown, 0,
                                         (byte)((initialAngle & 0xFF00) >> 8), (byte)initialAngle,
                                         (byte)((x & 0xFF00) >> 8), (byte)x,
                                         (byte)((y & 0xFF00) >> 8), (byte)y,
                                         (byte)((z & 0xFF00) >> 8), (byte)z);
            }
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
