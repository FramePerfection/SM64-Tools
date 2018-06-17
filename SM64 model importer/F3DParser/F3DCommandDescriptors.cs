using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter.F3DParser
{
    public class F3DCommandDescriptors
    {
        public static Dictionary<string, ulong> enumValues = new Dictionary<string, ulong>();
        public static Dictionary<string, CommandDescription> descriptions = new Dictionary<string, CommandDescription>();
        static F3DCommandDescriptors()
        {
            #region command descriptors
            descriptions["G_SPNOOP"] = new CommandDescription(0x00);

            descriptions["G_MTX"] = new CommandDescription(0x01,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Pointer },
                (opCode, args) => new DisplayList.Command(opCode, (int)((args[0] & 0xFF) << 0x10), (int)args[1]));

            descriptions["G_MOVEMEM"] = new CommandDescription(0x03,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Pointer },
                (opCode, args) => new DisplayList.Command(opCode, ((((args[0] / 8) + 1) * 8) << 0x10) | ((byte)args[1] << 0x8) | (byte)args[2], (int)args[3]));

            descriptions["G_VTX"] = new CommandDescription(0x04,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Pointer },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0xF) << 0x14) | ((args[1] & 0xF) << 0x10) | (short)args[2], args[3]));

            descriptions["G_DL"] = new CommandDescription(0x06,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Pointer },
                (opCode, args) => new DisplayList.Command(opCode, (byte)args[0] << 0x10, (int)args[1]));

            descriptions["G_RDPHALF_2"] = new CommandDescription(0xB3,
                new ArgumentType[] { ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, (int)args[0]));

            descriptions["G_RDPHALF_1"] = new CommandDescription(0xB4,
                new ArgumentType[] { ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, (int)args[0]));

            descriptions["G_QUAD"] = new CommandDescription(0xB5,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, (byte)(args[0] * 0xA), (byte)(args[1] * 0xA), (byte)(args[2] * 0xA), 0, (byte)(args[3] * 0xA), (byte)(args[4] * 0xA), (byte)(args[5] * 0xA)));

            descriptions["G_CLEARGEOMETRYMODE"] = new CommandDescription(0xB6,
                new ArgumentType[] { ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_SETGEOMETRYMODE"] = new CommandDescription(0xB7,
                new ArgumentType[] { ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_ENDDL"] = new CommandDescription(0xB8);

            descriptions["G_SetOtherMode_L"] = new CommandDescription(0xB9,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((byte)args[0] << 0x8) | (byte)args[1], args[2]));

            descriptions["G_SetOtherMode_H"] = new CommandDescription(0xBA,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((byte)args[0] << 0x8) | (byte)args[1], args[2]));

            descriptions["G_TEXTURE"] = new CommandDescription(0xBB,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 7) << 0xB) | ((args[1] & 0x7) << 0x8) | (byte)args[2], ((short)args[3] << 0x10) | (short)args[4]));

            descriptions["G_MOVEWORD"] = new CommandDescription(0xBC,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((byte)args[0] << 0x10) | (short)args[1], args[2]));

            descriptions["G_POPMTX"] = new CommandDescription(0xBD,
                new ArgumentType[] { ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0x380002, args[0]));

            descriptions["G_CULLDL"] = new CommandDescription(0xBE,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, (short)args[0], (short)args[1]));

            descriptions["G_TRI1"] = new CommandDescription(0xBF,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, 0, 0, 0, args[0], args[1], args[2]));

            descriptions["G_NOOP"] = new CommandDescription(0xC0);

            descriptions["G_TEXRECT"] = new CommandDescription(0xE4); //Unsupported
            descriptions["G_TEXRECTFLIP"] = new CommandDescription(0xE5); //Unsupported

            descriptions["G_RDPLOADSYNC"] = new CommandDescription(0xE6);

            descriptions["G_RDPPIPESYNC"] = new CommandDescription(0xE7);

            descriptions["G_RDPTILESYNC"] = new CommandDescription(0xE8);

            descriptions["G_RDPFULLSYNC"] = new CommandDescription(0xE9);

            descriptions["G_SETKEYGB"] = new CommandDescription(0xEA,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0xFFF) << 0xC) | (args[1] & 0xFFF), ((byte)args[2] << 0x18) | ((byte)args[3] << 0x10) | ((byte)args[4] << 0x8) | (byte)args[5]));

            descriptions["G_SETKEYR"] = new CommandDescription(0xEB,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, ((args[0] & 0xFFF) << 0x10) | ((byte)args[1] << 8) | (byte)args[2]));

            descriptions["G_SETCONVERT"] = new CommandDescription(0xEC,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((((args[0] & 0x1FF) << 9) | (args[1] & 0x1FF)) << 4) | ((args[2] & 0x1FF) >> 5), ((((((args[2] & 0x1F) << 9) | (args[3] & 0x1FF)) << 9) | (args[4] & 0x1FF)) << 9) | (args[5] & 0x1FF)));

            descriptions["G_SETSCISSOR"] = new CommandDescription(0xED,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((short)args[0] << 0x10) | (short)args[1], (args[2] & 3) << 0x1C | ((args[3] & 0xFFF) << 0xC) | (args[4] & 0xFFF)));

            descriptions["G_SETPRIMDEPTH"] = new CommandDescription(0xEE,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, ((short)args[0] << 0x10) | (short)args[1]));

            descriptions["G_RDPSetOtherMode"] = new CommandDescription(0xEF,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, args[0] & 0xFFFFFF, args[1]));

            descriptions["G_LOADTLUT"] = new CommandDescription(0xF0,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, ((args[0] & 0xF) << 0x10) | ((args[1] & 0xFFF) << 0xC)));

            descriptions["G_SETTILESIZE"] = new CommandDescription(0xF2,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, 0, ((args[0] & 0xFFF) << 0xC) | (args[1] & 0xFFF)));

            descriptions["G_LOADBLOCK"] = new CommandDescription(0xF3,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0xFFF) << 0xC) | (args[1] & 0xFFF), ((args[2] & 0xF) << 0x18) | ((args[3] & 0xFFF) << 0xC) | (args[4] & 0xFFF)));

            descriptions["G_LOADTILE"] = new CommandDescription(0xF4,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0xFFF) << 0xC) | (args[1] & 0xFFF), ((args[2] & 0xF) << 0x18) | ((args[3] & 0xFFF) << 0xC) | (args[4] & 0xFFF)));

            descriptions["G_SETTILE"] = new CommandDescription(0xF5,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 7) << 0x15) | ((args[1] & 3) << 0x13) | ((args[2] & 0x1F) << 0x9) | (args[3] & 0x1F), ((args[4] & 7) << 0x18) | ((args[5] & 0xF) << 0x14) | ((args[6] & 3) << 0x12) | ((args[7] & 0xF) << 0xE) | ((args[8] & 0xF) << 0xA) | ((args[9] & 3) << 0x8) | ((args[10] & 0xF) << 0x4) | (args[11] & 0xF)));

            descriptions["G_FILLRECT"] = new CommandDescription(0xF6,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0xFFF) << 0xC) | (args[1] & 0xFFF), ((args[2] & 0xFFF) << 0xC) | (args[3] & 0xFFF)));

            descriptions["G_SETFILLCOLOR"] = new CommandDescription(0xF7,
                new ArgumentType[] { ArgumentType.Color },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_SETFOGCOLOR"] = new CommandDescription(0xF8,
                new ArgumentType[] { ArgumentType.Color },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_SETBLENDCOLOR"] = new CommandDescription(0xF9,
                new ArgumentType[] { ArgumentType.Color },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_SETPRIMCOLOR"] = new CommandDescription(0xFA,
                new ArgumentType[] { ArgumentType.Color },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_SETENVCOLOR"] = new CommandDescription(0xFB,
                new ArgumentType[] { ArgumentType.Color },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_SETCOMBINE"] = new CommandDescription(0xFC,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Numeric },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0xF) << 0x14) | ((args[1] & 0x1F) << 0xF) | ((args[2] & 0x7) << 0xC) | ((args[3] & 0x3) << 0x9) | ((args[4] & 0xF) << 0x5) | (args[5] & 0x1F),
                    ((args[6] & 0xF) << 0x1C) | ((args[7] & 0xF) << 0x18) | ((args[8] & 0x7) << 0x15) | ((args[9] & 0x7) << 0x12) | ((args[10] & 0x7) << 0xF) | ((args[11] & 0x7) << 0xC) | ((args[12] & 0x7) << 0x9) | ((args[13] & 0x7) << 0x6) | ((args[14] & 0x7) << 0x3) | (args[15] & 0x7)));
            
            descriptions["G_SETTIMG"] = new CommandDescription(0xFD,
                new ArgumentType[] {ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Pointer},
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0x7) << 0x15) | ((args[1] & 0x3) << 0x13), args[2]));

            descriptions["G_SETZIMG"] = new CommandDescription(0xFE,
                new ArgumentType[] { ArgumentType.Pointer },
                (opCode, args) => new DisplayList.Command(opCode, 0, args[0]));

            descriptions["G_SETCIMG"] = new CommandDescription(0xFF,
                new ArgumentType[] { ArgumentType.Numeric, ArgumentType.Numeric, ArgumentType.Pointer },
                (opCode, args) => new DisplayList.Command(opCode, ((args[0] & 0x7) << 0x15) | ((args[1] & 0x3) << 0x13) | ((args[2]-1) & 0xFFF), args[3]));
            #endregion

            #region enum values
            enumValues["G_MTX_MODELVIEW"] = 0;
            enumValues["G_MTX_PROJECTION"] = 1;
            enumValues["G_MTX_MUL"] = 0;
            enumValues["G_MTX_LOAD"] = 2;
            enumValues["G_MTX_NOPUSH"] = 0;
            enumValues["G_MTX_PUSH"] = 4;
            #endregion
        }
    }
}
