using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SM64RAM;

namespace SM64ModelImporter
{
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
        //public string comment = "";

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
            UpdateSizeInfo();
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
            while ((baseS & 1) == 0 && sS < 5 && false)
            { sS++; baseS >>= 1; }

            int baseT = height;
            int sT = 0;
            while ((baseT & 1) == 0 && sT < 5 && false)
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

            int tile  = 0;
            int tmemOffset = 0;
            bool ignoreShift = states.RCP_TexGen == RenderStates.RCP_OP.set ||  states.RCP_TexGenLinear == RenderStates.RCP_OP.set;
            int shiftParamS = ignoreShift ? 0 : (16 - shiftS);
            int shiftParamT = ignoreShift ? 0 : (16 - shiftT);

            customClampX = 4 * (width - 1);
            customClampY = 4 * (height - 1);

            targetList.Add(Commands.G_SETTILE(TextureImage.TextureFormat.G_IM_FMT_RGBA, usedBpp, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0));
            targetList.Add(new DisplayList.Command(0xE8)); //G_RDPTILESYNC
            if (usedFormat == TextureImage.TextureFormat.G_IM_FMT_CI)
            {
                targetList.Add(Commands.G_SETTILE(TextureImage.TextureFormat.G_IM_FMT_RGBA, TextureImage.BitsPerPixel.G_IM_SIZ_4b, 0, 0x100, tile, 0, AddressMode.G_TX_WRAP, 0, 0, AddressMode.G_TX_WRAP, 0, 0));
                targetList.Add(Commands.G_SETIMG(TextureImage.TextureFormat.G_IM_FMT_RGBA, TextureImage.BitsPerPixel.G_IM_SIZ_16b, 1, usedOffset));
                targetList.Add(Commands.G_LOADTLUT(tile, 1 << actualBpp));
                targetList.Add(new DisplayList.Command(0xBA, 0x000E02, 0x00008000));
                usedOffset += 2 << actualBpp;
            }

            //This does not seem to be correct for "line" parameter in G_SETTILE with some graphics plugins
            int line = (Math.Min(0x10, actualBpp) * width + 63) / 64;
            int txl2words = Math.Max(1, (width * actualBpp / 64));
            int dxt = ((1 << 0xB) + txl2words - 1) / txl2words;

            wrapExponentX = (int)Math.Log(width, 2);
            wrapExponentY = (int)Math.Log(height, 2);
            targetList.Add(Commands.G_SETTILE(usedFormat, usedBpp, line, tmemOffset, tile, 0, addressY, wrapExponentY, shiftParamT, addressX, wrapExponentX, shiftParamS));

            //Scrolling textures are achieved changing the uls and ult parameters of G_SETTILESIZE
            scrollingTextureRelativePointer = targetList.Count * 8;
            targetList.Add(Commands.G_SETTILESIZE(tile, 0, 0, customClampX, customClampY));
            targetList.Add(Commands.G_SETIMG(usedFormat, usedBpp, width, usedOffset));

            targetList.Add(new DisplayList.Command(0xE6)); //G_RDPLOADSYNC
            targetList.Add(Commands.G_LOADBLOCK(7, 0, 0, width * height, dxt));


            targetList.Add(new DisplayList.Command(0x03, 0x860010, materialOffset + materialIndex * 0x20)); //Light 1
            targetList.Add(new DisplayList.Command(0x03, 0x880010, materialOffset + materialIndex * 0x20 + 0x10)); //Light 2
        }

        public void AppendResetCommands(List<DisplayList.Command> targetList)
        {
            TextureImage.TextureFormat usedFormat = isCustom ? customFormat : image.format;
            if (usedFormat == TextureImage.TextureFormat.G_IM_FMT_CI)
            {
                targetList.Add(new DisplayList.Command(0xE7));
                targetList.Add(new DisplayList.Command(0xBA, 0x000E02, 0x00000000));
            }
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
