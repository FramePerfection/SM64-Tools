using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace SM64Renderer
{
    public enum BitsPerPixel
    {
        G_IM_SIZ_4b = 0,
        G_IM_SIZ_8b = 1,
        G_IM_SIZ_16b = 2,
        G_IM_SIZ_32b = 3
    }
    public enum TextureFormat
    {
        G_IM_FMT_RGBA = 0,
        G_IM_FMT_YUV = 1,
        G_IM_FMT_CI = 2,
        G_IM_FMT_IA = 3,
        G_IM_FMT_I = 4
    }

    public struct TextureInfo
    {
        public int segmentedPointer;
        public int width, height;
        public int bpp, fmt;
        public int texels, dxt;
        public int pallete;

        public int GetActualBpp() { return 4 << bpp; }
    }
    public class TextureManager
    {
        public struct Tile
        {
            public Texture texture;
            public int uls, ult;
            public int lrs, lrt;
        }

        public Tile[] tiles = new Tile[8];

        public Dictionary<TextureInfo, Texture> textures = new Dictionary<TextureInfo, Texture>();
        DeviceWrapper device    ;
        EmulationState state;
        public TextureManager(EmulationState state, DeviceWrapper device)
        {
            this.device = device;
            this.state = state;
        }

        public Texture LoadByInfo(TextureInfo info)
        {
            Texture result;
            if (textures.TryGetValue(info, out result))
                return result;
            Bitmap bmp = null;
            if (info.width == 0) return null;
            if (info.fmt == (int)TextureFormat.G_IM_FMT_RGBA)
                if (info.bpp == (int)BitsPerPixel.G_IM_SIZ_16b)
                    bmp = ImageConverter.ReadRGBA16(state.banks[info.segmentedPointer >> 0x18].value, info.segmentedPointer & 0xFFFFFF, info.width, info.height);
                else if (info.bpp == (int)BitsPerPixel.G_IM_SIZ_32b)
                    bmp = ImageConverter.ReadRGBA32(state.banks[info.segmentedPointer >> 0x18].value, info.segmentedPointer & 0xFFFFFF, info.width, info.height);
            if (bmp == null) return null;
            Texture newTex = new Texture(device, bmp, Usage.None, Pool.Managed);
            textures[info] = newTex;
            return newTex;
        }
    }
}
