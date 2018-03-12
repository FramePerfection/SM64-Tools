using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter
{
    public static class Commands
    {
        public static DisplayList.Command G_LOADTLUT(int tile, int count)
        {
            int lF0 = (tile << 0x18) | (((count - 1) & 0x3FF) << 2 << 0xC);
            return new DisplayList.Command(0xF0, 0, lF0);
        }

        public static DisplayList.Command G_SETTILESIZE(int tile, int uls, int ult, int lrs, int lrt)
        {
            int uF2 = (uls << 0xC) | (ult & 0xFFF);
            int lF2 = (tile << 0x18) |
                        (lrs << 0xC) |
                        lrt;
            return new DisplayList.Command(0xF2, uF2, lF2);
        }

        public static DisplayList.Command G_LOADBLOCK(int tile, int uls, int ult, int texels, int dxt)
        {
            int uF3 = (uls << 0xC) | (ult & 0xFFF);
            int lF3 = (tile << 0x18) |
                        ((texels - 1) << 0xC) |
                        dxt;
            return new DisplayList.Command(0xF3, uF3, lF3);
        }

        public static DisplayList.Command G_SETTILE(TextureImage.TextureFormat fmt, TextureImage.BitsPerPixel size, int line, int tmem, int tile, int pallete, TextureInfo.AddressMode cmS, int maskS, int shiftS, TextureInfo.AddressMode cmT, int maskT, int shiftT)
        {
            int uF5 = ((int)fmt << 0x15) |
                        ((int)size << 0x13) |
                        (line << 0x9) |
                        tmem;
            int lF5 = (tile << 0x18) |
                        (pallete << 0x14) |
                        ((int)cmS << 0x12) | (maskS << 0xE) | (shiftS << 0xA) |
                        ((int)cmT << 0x8) | (maskT << 0x4) | shiftT;
            return new DisplayList.Command(0xF5, uF5, lF5);
        }

        public static DisplayList.Command G_SETIMG(TextureImage.TextureFormat fmt, TextureImage.BitsPerPixel size, int width, int segmentedPointer)
        {
            int uFD = ((int)fmt << 0x15) |
                                ((int)size << 0x13) |
                                (width - 1);
            return new DisplayList.Command(0xFD, uFD, segmentedPointer);
        }
    }
}
