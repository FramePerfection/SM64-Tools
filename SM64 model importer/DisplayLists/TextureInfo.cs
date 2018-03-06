using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SM64RAM;

namespace SM64_model_importer
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
        public Bitmap file;
        public TextureFormat format = TextureFormat.G_IM_FMT_RGBA;
        public BitsPerPixel bpp = BitsPerPixel.G_IM_SIZ_16b;
        List<TextureInfo> users = new List<TextureInfo>();
        public string comment = "";

        public static TextureImage GetImageByName(string name, TextureInfo user)
        {
            TextureImage existing;
            if (Main.instance.textureLibrary.textures.TryGetValue(name, out existing))
            {
                existing.AddUser(user);
                return existing;
            }
            string comment;
            TextureImage newImage = new TextureImage(ImageConverter.FitBitmap(name, out comment), user);
            Main.instance.textureLibrary.textures.Add(name, newImage);
            newImage.comment = comment;
            return newImage;
        }

        private TextureImage(Bitmap bmp, TextureInfo user)
        {
            this.file = bmp;
            users.Add(user);
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

        public int GetSizeInBytes()
        {
            int actualBpp = 2 << (1 + (int)bpp);
            return (file.Width * file.Height * actualBpp + 7) / 8;
        }

        public void WriteBytes()
        {
            EmulationState.RAMBank target = EmulationState.instance.banks[segmentOffset >> 0x18];
            if (target == null)
                throw new Exception("Bad Texture");
            ImageConverter.ConvertRGBA(format, bpp, file, target.value, segmentOffset & 0xFFFFFF);
        }
    }

    public class TextureInfo : ReadWrite
    {
        public enum AddressMode
        {
            G_TX_MIRROR = 1,
            G_TX_WRAP = 0,
            G_TX_CLAMP = 2
        }

        public TextureImage image;
        Bitmap rom;
        public bool isCustom = false;
        public int customAddress { get; private set; }
        public int customClampX, customClampY;
        public int wrapExponentX, wrapExponentY;
        public int materialIndex = 0;
        public AddressMode addressX = AddressMode.G_TX_WRAP, addressY = AddressMode.G_TX_WRAP;
        public string comment = "";

        public int shiftS { get; private set; }
        public int shiftT { get; private set; }
        public int baseMultiplierS { get; private set; }
        public int baseMultiplierT { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int customWidth = 0x20, customHeight = 0x20;
        public TextureImage.BitsPerPixel customBpp = TextureImage.BitsPerPixel.G_IM_SIZ_16b;
        public TextureImage.TextureFormat customFormat = TextureImage.TextureFormat.G_IM_FMT_RGBA;

        //Parameters for scrolling textures
        public int scrollingTextureRelativePointer { get; private set; } //Offset of the 0xF2 command relative to the start of the display list it's in.
        public int[] stRAMPointers = new int[0], stROMPointers = new int[0]; //Addresses in RAM / ROM that shall point to the 0xF2 command

        public TextureInfo(string name)
        {
            if (name == null)
            {
                isCustom = true;
                UpdateCustom();
            }
            else
            {
                image = TextureImage.GetImageByName(name, this);
                UpdateSizeInfo();
            }
            customAddress = 0x07000000;
        }

        public Bitmap GetBitmap()
        {
            return (isCustom) ? rom : image.file;
        }

        public void SetCustomAddress(int newAddress)
        {
            int segmentNumber = newAddress >> 0x18;
            if (segmentNumber >= EmulationState.instance.banks.Length || segmentNumber < 0) return;
            EmulationState.RAMBank bank = EmulationState.instance.banks[newAddress >> 0x18];
            if (bank == null) return;
            customAddress = newAddress;
            UpdateCustom();
        }

        public void SetFormat(TextureImage.TextureFormat format, TextureImage.BitsPerPixel bpp)
        {
            if (isCustom)
            {
                customFormat = format;
                customBpp = bpp;
            }
            else
            {
                image.bpp = bpp;
                image.format = format;
            }
        }

        public void UpdateCustom()
        {
            int segmentNumber = customAddress >> 0x18;
            EmulationState.RAMBank bank = EmulationState.instance.banks[segmentNumber];
            if (bank == null || !EmulationState.instance.AssertRead(customAddress, customWidth * customHeight * (4 << (int)customBpp)))
                rom = new Bitmap(customWidth, customHeight);
            else
                rom = ImageConverter.ReadRGBA16(bank.value, customAddress & 0xFFFFFF, customWidth, customHeight);
            UpdateSizeInfo();
        }

        void UpdateSizeInfo()
        {
            Bitmap usedMap = GetBitmap();
            if (usedMap == null)
            { width = customWidth; height = customHeight; }
            else
            { width = usedMap.Width; height = usedMap.Height; }
            int baseS = width;
            int sS = 0;
            while ((baseS & 1) == 0 && sS < 5)
            { sS++; baseS >>= 1; }

            int baseT = height;
            int sT = 0;
            while ((baseT & 1) == 0 && sT < 5)
            { sT++; baseT >>= 1; }
            shiftS = sS;
            shiftT = sT;
            baseMultiplierS = baseS;
            baseMultiplierT = baseT;
        }

        public void AppendCommands(List<DisplayList.Command> targetList, int materialOffset, RenderStates states)
        {
            TextureImage.BitsPerPixel usedBpp = isCustom ? customBpp : image.bpp;
            TextureImage.TextureFormat usedFormat = isCustom ? customFormat : image.format;
            int usedOffset = isCustom ? customAddress : image.segmentOffset;
            int actualBpp = 4 << (int)usedBpp;

            int tile = 0;
            int pallete = 0;
            int tmemOffset = 0;
            bool ignoreShift = states.RCP_TexGen || states.RCP_TexGenLinear;
            int shiftParamS = ignoreShift ? 0 : 16 - shiftS;
            int shiftParamT = ignoreShift ? 0 : 16 - shiftT;

            customClampX = 4 * (width - 1);
            customClampY = 4 * (height - 1);

            //This does not seem to be correct for "line" parameter in G_SETTILE with some graphics plugins
            int line = (actualBpp * width + 63) / 64;

            wrapExponentX = (int)Math.Log(width, 2);
            wrapExponentY = (int)Math.Log(height, 2);
            targetList.Add(new DisplayList.Command(0xE8)); //G_RDPTILESYNC
            targetList.Add(Commands.G_SETIMG(usedFormat, usedBpp, width, usedOffset));
            targetList.Add(Commands.G_SETTILE(usedFormat, usedBpp, line, tmemOffset, tile, pallete, addressY, wrapExponentY, shiftParamT, addressX, wrapExponentX, shiftParamS));


            int txl2words = Math.Max(1, (width * actualBpp / 64));
            int dxt = ((1 << 0xB) + txl2words - 1) / txl2words;
            targetList.Add(new DisplayList.Command(0xE6)); //G_RDPLOADSYNC
            targetList.Add(Commands.G_LOADBLOCK(tile, 0, 0, width * height, dxt));

            //Scrolling textures are achieved changing the uls and ult parameters of G_SETTILESIZE
            scrollingTextureRelativePointer = targetList.Count * 8;
            targetList.Add(Commands.G_SETTILESIZE(tile, 0, 0, customClampX, customClampY));

            targetList.Add(new DisplayList.Command(0x03, 0x860010, materialOffset + materialIndex * 0x10)); //Light 1
            targetList.Add(new DisplayList.Command(0x03, 0x880010, materialOffset + materialIndex * 0x10 + 8)); //Light 2
        }

        public void LoadSettings(FileParser.Block block)
        {
            addressX = (AddressMode)block.GetInt("Address X");
            addressY = (AddressMode)block.GetInt("Address Y");
            stRAMPointers = block.GetIntArray("RAM Pointers", false);
            stROMPointers = block.GetIntArray("ROM Pointers", false);
        }
        public void SaveSettings(FileParser.Block block)
        {
            block.SetInt("Address X", (int)addressX, false);
            block.SetInt("Address Y", (int)addressY, false);
            block.SetIntArray("RAM Pointers", stRAMPointers);
            block.SetIntArray("ROM Pointers", stROMPointers);
        }
    }
}
