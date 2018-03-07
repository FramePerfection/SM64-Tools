using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace SM64_model_importer
{
    class ImageConverter
    {
        delegate void PixelDataWriter(ref int cursor, byte[] outputData, int r, int g, int b, int a);
        static PixelDataWriter[,] writers = new PixelDataWriter[5, 4];
        static bool halfByte = false;

        static ImageConverter()
        {
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_RGBA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_16b] = WriteRGBA16;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_RGBA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_32b] = WriteRGBA32;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_IA, (int)TextureImage.BitsPerPixel.G_IM_SIZ_16b] = WriteIA16;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_I, (int)TextureImage.BitsPerPixel.G_IM_SIZ_4b] = WriteI4;
            writers[(int)TextureImage.TextureFormat.G_IM_FMT_I, (int)TextureImage.BitsPerPixel.G_IM_SIZ_8b] = WriteI8;
        }

        public static Bitmap FitBitmap(string sourceFile, out string comment)
        {
            bool makeTilable = true;

            Bitmap source = (Bitmap)Bitmap.FromFile(sourceFile);
            comment = "Texture is fine.";
            if (source.Width * source.Height <= 0x800)
                return source;

            int newWidth = source.Width;
            int newHeight = source.Height;
            while (newWidth * newHeight > 0x800)
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
            if (newWidth * newHeight > 0x800)
                EmulationState.messages.AppendMessage.MessageBox.Show("Failed to scale image down D:", "This should really not happen.");

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
            source.Dispose();
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

        public static void ConvertRGBA(TextureImage.TextureFormat format, TextureImage.BitsPerPixel bpp, Bitmap source, byte[] outputStream, int cursor)
        {
            //Convert image to RGBA if necessary
            Bitmap RGBAconvert = source.PixelFormat == PixelFormat.Format32bppArgb ? source : new Bitmap(source.Width, source.Height, PixelFormat.Format32bppPArgb);
            if (source.PixelFormat != PixelFormat.Format32bppArgb)
                using (Graphics gr = Graphics.FromImage(RGBAconvert))
                    gr.DrawImage(source, new Rectangle(0, 0, RGBAconvert.Width, RGBAconvert.Height));

            //Put image data into pixelData
            BitmapData data = RGBAconvert.LockBits(new Rectangle(0, 0, RGBAconvert.Width, RGBAconvert.Height), ImageLockMode.ReadOnly, RGBAconvert.PixelFormat);
            int stride = data.Stride / 4, width = data.Width, height = data.Height;
            byte[] pixelData = new byte[stride * data.Height * 4];
            Marshal.Copy(data.Scan0, pixelData, 0, pixelData.Length);
            RGBAconvert.UnlockBits(data);
            //RGBAconvert.Dispose();

            PixelDataWriter writer = writers[(int)format, (int)bpp];
            if (writer == null)
                throw new Exception("Unsupported format " + format.ToString() + " / " + bpp.ToString() + "!");

            //Write each pixel into the output stream
            halfByte = false;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int i = (x + stride * (height - (y + 1))) * 4;
                    writer(ref cursor, outputStream, pixelData[i + 2], pixelData[i + 1], pixelData[i + 0], pixelData[i + 3]);
                }
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
    }
}
