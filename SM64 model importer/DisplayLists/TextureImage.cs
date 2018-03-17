using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SM64RAM;

namespace SM64ModelImporter
{
    public class TextureImage
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


        public int segmentOffset;
        public Bitmap file { get { return scaled; } }
        TextureFormat _format = TextureFormat.G_IM_FMT_RGBA;
        public TextureFormat format { get { return _format; } set { _format = value; UpdateScaledBitmap(); } }
        BitsPerPixel _bpp = BitsPerPixel.G_IM_SIZ_16b;
        public BitsPerPixel bpp { get { return _bpp; } set { _bpp = value; UpdateScaledBitmap(); } }
        List<TextureInfo> users = new List<TextureInfo>();
        public string comment = "Unknown";
        Bitmap source, scaled;

        public static TextureImage GetImageByName(string name, TextureInfo user)
        {
            TextureImage existing;
            if (Main.instance.textureLibrary.textures.TryGetValue(name, out existing))
            {
                existing.AddUser(user);
                return existing;
            }
            Bitmap source = (Bitmap)Bitmap.FromFile(name);
            TextureImage clone = new TextureImage(new Bitmap(source), user);
            source.Dispose();
            Main.instance.textureLibrary.textures.Add(name, clone);
            return clone;
        }

        private TextureImage(Bitmap bmp, TextureInfo user)
        {
            source = bmp;
            UpdateScaledBitmap();
            users.Add(user);
        }

        void UpdateScaledBitmap()
        {
            scaled = ImageConverter.FitBitmap(source, (format == TextureFormat.G_IM_FMT_CI ? 0x1000 : 0x2000) / (1 << (int)_bpp), out comment);
        }

        public void AddUser(TextureInfo user)
        {
            if (!users.Contains(user))
                users.Add(user);
        }

        public void RemoveUser(TextureInfo user)
        {
            users.Remove(user);
            if (users.Count == 0)
            {
                Main.instance.textureLibrary.Remove(this);
            }
        }

        public void WriteBytes(ref int cursor)
        {
            EmulationState.RAMBank target = EmulationState.instance.banks[segmentOffset >> 0x18];
            int cursorInBank = cursor & 0xFFFFFF;
            int bank = (int)(cursor & 0xFF000000);
            if (target == null)
                throw new Exception("Bad Texture");
            ImageConverter.ConvertRGBA(format, bpp, file, target.value, ref cursorInBank);
            cursor = bank | cursorInBank;
        }
    }

}
