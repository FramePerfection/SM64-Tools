using System;
using System.Collections.Generic;
using System.Text;

namespace SM64Renderer
{
    public enum GBI : byte
    {
        G_SETCIMG = 0xFF,
        G_SETZIMG = 0xFE,
        G_SETTIMG = 0xFD,
        G_SETCOMBINE = 0xFC,
        G_SETENVCOLOR = 0xFB,
        G_SETPRIMCOLOR = 0xFA,
        G_SETBLENDCOLOR = 0xF9,
        G_SETFOGCOLOR = 0xF8,
        G_SETFILLCOLOR = 0xF7,
        G_FILLRECT = 0xF6,
        G_SETTILE = 0xF5,
        G_LOADTILE = 0xF4,
        G_LOADBLOCK = 0xF3,
        G_SETTILESIZE = 0xF2,
        G_LOADTLUT = 0xF0,
        G_RDPSETOTHERMODE = 0xEF,
        G_SETPRIMDEPTH = 0xEE,
        G_SETSCISSOR = 0xED,
        G_SETCONVERT = 0xEC,
        G_SETKEYR = 0xEB,
        G_SETKEYGB = 0xEA,
        G_RDPFULLSYNC = 0xE9,
        G_RDPTILESYNC = 0xE8,
        G_RDPPIPESYNC = 0xE7,
        G_RDPLOADSYNC = 0xE6,
        G_TEXRECTFLIP = 0xE5,
        G_TEXRECT = 0xE4,

        G_RDPNOOP = 0xC0,

        G_TRI_FILL = 0xC8,
        G_TRI_SHADE = 0xCC,
        G_TRI_TXTR = 0xCA,
        G_TRI_SHADE_TXTR = 0xCE,
        G_TRI_FILL_ZBUFF = 0xC9,
        G_TRI_SHADE_ZBUFF = 0xCD,
        G_TRI_TXTR_ZBUFF = 0xCB,
        G_TRI_SHADE_TXTR_ZBUFF = 0xCF
    }

    public enum F3D : byte
    {
        SPNOOP = 0x00,
        MTX = 0x01,
        RESERVED0 = 0x02,
        MOVEMEM = 0x03,
        VTX = 0x04,
        RESERVED1 = 0x05,
        DL = 0x06,
        RESERVED2 = 0x07,
        RESERVED3 = 0x08,
        SPRITE2D_BASE = 0x09,

        TRI1 = 0xBF,
        CULLDL = 0xBE,
        POPMTX = 0xBD,
        MOVEWORD = 0xBC,
        TEXTURE = 0xBB,
        SETOTHERMODE_H = 0xBA,
        SETOTHERMODE_L = 0xB9,
        ENDDL = 0xB8,
        SETGEOMETRYMODE = 0xB7,
        CLEARGEOMETRYMODE = 0xB6,

        QUAD = 0xB5,
        RDPHALF_1 = 0xB4,
        RDPHALF_2 = 0xB3,
        RDPHALF_CONT = 0xB2,
        TRI4 = 0xB1
    }
}
