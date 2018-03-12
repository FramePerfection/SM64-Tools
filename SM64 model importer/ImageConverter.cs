using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using SM64RAM;
using nQuant;

namespace SM64ModelImporter
{
    class ImageConverter
    {
        delegate void PixelDataWriter(ref int cursor, byte[] outputData, int r, int g, int b, int a);
        static PixelDataWriter[,] writers = new PixelDataWriter[5, 4];
        static bool halfByte = false;
        static WuQuantizer quantizer = new WuQuantizer();

        static ImageConverter()
        {
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_RGBA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_16b] = WriteRGBA16;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_RGBA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_32b] = WriteRGBA32;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_IA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_16b] = WriteIA16;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_I, (int)TextureImage.BitsPerPixel.G_IM_SIZ_4b] = WriteI4;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_I, (int)TextureImage.BitsPerPixel.G_IM_SIZ_8b] = WriteI8;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_IA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_4b] = WriteIA4;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_IA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_8b] = WriteIA8;
        }

        public static Bitmap FitBitmap(Bitmap source, int maxSize, out string comment)
        {
            bool makeTilable = true;
            comment = "Texture is fine.";
            if (source.Width * source.Height <= maxSize)
                return source;

            int newWidth = source.Width;
            int newHeight = source.Height;
            while (newWidth * newHeight > maxSize)
            {
                if (!makeTilable)
                {
                    if (newWidth > newHeight)
                    {
                        newWidth--;
                        newHeight = (int)(source.Height * (float)newWidth / source.Width + 0.5f);
                    }
                    else
                    {
                        newHeight--;
                        newWidth = (int)(source.Width * (float)newHeight / source.Height + 0.5f);
                    }
                }
                else
                {
                    newWidth = 1 << ((int)Math.Log(newWidth - 1, 2));
                    newHeight = 1 << ((int)Math.Log(newHeight - 1, 2));
                }
            }
            comment = "Image is too large (" + source.Width.ToString() + " x " + source.Height.ToString() + ") and has to be resized to " + newWidth.ToString() + " x " + newHeight.ToString();
            if (newWidth * newHeight > maxSize)
                EmulationState.messages.AppendMessage("Failed to scale image down D:", "This should really not happen");

            Rectangle destRect = new Rectangle(0, 0, newWidth, newHeight);
            Bitmap destImage = new Bitmap(newWidth, newHeight);

            destImage.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(source, destRect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        public static Bitmap ReadRGBA16(byte[] bank, int offset, int width, int height)
        {
            byte[] outputImage = new byte[width * height * 4];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int i = offset + (x + y * width) * 2;
                    byte b1 = bank[i], b2 = bank[i + 1];
                    byte A = (byte)((b2 & 0x1) > 0 ? 255 : 0), R = (byte)(b1 & 0xF8), G = (byte)((b1 & 0x7) << 5 | ((b2 & 0xc0) >> 6)), B = (byte)((b2 & 0x3E) << 2);
                    byte v = (byte)((R + G + B) / 3);

                    int outputOffset = (x + y * width) * 4;
                    outputImage[outputOffset + 3] = A; //A
                    outputImage[outputOffset + 2] = R; //R
                    outputImage[outputOffset + 1] = G; //G
                    outputImage[outputOffset + 0] = B; //B
                }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData asfas = bmp.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(outputImage, 0, asfas.Scan0, outputImage.Length);
            bmp.UnlockBits(asfas);
            return bmp;
        }

        public static void ConvertRGBA(TextureImage.TextureFormat format, TextureImage.BitsPerPixel bpp, Bitmap source, byte[] outputStream, ref int cursor)
        {
            //Convert image to RGBA if necessary
            bool createRGBA_convert = source.PixelFormat != PixelFormat.Format32bppArgb;
            Bitmap RGBAconvert = createRGBA_convert ? new Bitmap(source.Width, source.Height, PixelFormat.Format32bppPArgb) : source;
            if (source.PixelFormat != PixelFormat.Format32bppArgb)
                using (Graphics gr = Graphics.FromImage(RGBAconvert))
                    gr.DrawImage(source, new Rectangle(0, 0, RGBAconvert.Width, RGBAconvert.Height));


            if (format == TextureImage.TextureFormat.G_IM_FMT_CI)
            {
                int colors = bpp == TextureImage.BitsPerPixel.G_IM_SIZ_4b ? 16 : 256;
                Bitmap quantized = (Bitmap)quantizer.QuantizeImage(RGBAconvert, colors);
                for (int i = 0; i < colors; i++)
                {
                    Color c = quantized.Palette.Entries[i];
                    WriteRGBA16(ref cursor, outputStream, c.R, c.G, c.B, c.A);
                }
                cursor = ((cursor + 0xF) / 0x10) * 0x10; //Align cursor to multiple of 16 bytes (8 would probably work fine, too)
                //Put image data into pixelData
                BitmapData data = quantized.LockBits(new Rectangle(0, 0, RGBAconvert.Width, RGBAconvert.Height), ImageLockMode.ReadOnly, quantized.PixelFormat);
                int stride = data.Stride, width = data.Width, height = data.Height;
                byte[] indexData = new byte[stride * data.Height];
                Marshal.Copy(data.Scan0, indexData, 0, indexData.Length);
                quantized.UnlockBits(data);
                //Write each pixel into the output stream
                if (bpp == TextureImage.BitsPerPixel.G_IM_SIZ_4b)
                    for (int y = 0; y < height; y++)
                        for (int x = 0; x < width; x += 2)
                            outputStream[cursor++] = (byte)((indexData[y * stride + x] << 0x4) | indexData[y * stride + x + 1]);
                else
                    for (int y = 0; y < height; y++)
                        for (int x = 0; x < width; x++)
                            outputStream[cursor++] = indexData[y * stride + x];
            }
            else
            {
                //Put image data into pixelData
                BitmapData data = RGBAconvert.LockBits(new Rectangle(0, 0, RGBAconvert.Width, RGBAconvert.Height), ImageLockMode.ReadOnly, RGBAconvert.PixelFormat);
                int stride = data.Stride / 4, width = data.Width, height = data.Height;
                byte[] rgbaData = new byte[stride * data.Height * 4];
                Marshal.Copy(data.Scan0, rgbaData, 0, rgbaData.Length);
                RGBAconvert.UnlockBits(data);
                PixelDataWriter writer = writers[(int)format, (int)bpp];
                if (writer == null)
                    throw new Exception("Unsupported format " + format.ToString() + " / " + bpp.ToString() + "!");
                //Write each pixel into the output stream
                halfByte = false;
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        int i = (x + stride * (height - (y + 1))) * 4;
                        writer(ref cursor, outputStream, rgbaData[i + 2], rgbaData[i + 1], rgbaData[i + 0], rgbaData[i + 3]);
                    }
            }
            if (createRGBA_convert)
                RGBAconvert.Dispose();
        }

        static void WriteRGBA16(ref int cursor, byte[] outputData, int r, int g, int b, int a)
        {
            byte b1 = (byte)(r & 0xf8 | (g >> 5));
            byte b2 = (byte)((g >> 3 << 6) | ((b >> 2) & 0x3E) | (a >> 7));
            outputData[cursor] = b1; outputData[cursor + 1] = b2;
            cursor += 2;
        }

        static void WriteRGBA32(ref int cursor, byte[] outputData, int r, int g, int b, int a)
        {
            outputData[cursor] = (byte)r;
            outputData[cursor + 1] = (byte)g;
            outputData[cursor + 2] = (byte)b;
            outputData[cursor + 3] = (byte)a;
            cursor += 4;
        }

        static void WriteIA16(ref int cursor, byte[] outputData, int r, int g, int b, int a)
        {
            outputData[cursor] = (byte)((r + g + b) / 3);
            outputData[cursor + 1] = (byte)a;
            cursor += 2;
        }

        static void WriteI4(ref int cursor, byte[] outputData, int r, int g, int b, int a)
        {
            int v = Math.Min(0xFF, ((r + g + b) / 3));
            if (halfByte)
                outputData[cursor++] |= (byte)(v >> 4);
            else
                outputData[cursor] = (byte)(v & 0xF0);
            halfByte = !halfByte;
        }

        static void WriteI8(ref int cursor, byte[] outputData, int r, int g, int b, int a)
        {
            int v = Math.Min(0xFF, ((r + g + b) / 3));
            outputData[cursor++] = (byte)v;
        }

        static void WriteIA8(ref int cursor, byte[] outputData, int r, int g, int b, int a)
        {
            int v = Math.Min(0xFF, ((r + g + b) / 3));
            int alpha = a / 16;
            outputData[cursor++] = (byte)((v & 0xF0) | (alpha & 0xF));
        }
        static void WriteIA4(ref int cursor, byte[] outputData, int r, int g, int b, int a)
        {
            int v = (r + g + b)/ (3 * 64);
            int alpha = a / 64;
            if (halfByte)
                outputData[cursor++] |= (byte)(((v & 0x0C) | (alpha & 0x03)) << 4);
            else
                outputData[cursor] = (byte)((v & 0x0C) | (alpha & 0x03));
            halfByte = !halfByte;
        }
    }
}
