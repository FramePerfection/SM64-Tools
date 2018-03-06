using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SM64Renderer
{
    public class ImageConverter
    {
        public static Bitmap ReadRGBA16(byte[] bank, int offset, int width, int height)
        {
            byte[] outputImage = new byte[width * height * 4];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int i = offset + (x + y * width) * 2;
                    if (i >= bank.Length) return new Bitmap(1, 1);
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

        public static Bitmap ReadRGBA32(byte[] bank, int offset, int width, int height)
        {
            byte[] outputImage = new byte[width * height * 4];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int i = offset + (x + y * width) * 4;

                    int outputOffset = (x + y * width) * 4;
                    outputImage[outputOffset + 3] = bank[i + 3]; //A
                    outputImage[outputOffset + 2] = bank[i + 0]; //R
                    outputImage[outputOffset + 1] = bank[i + 1]; //G
                    outputImage[outputOffset + 0] = bank[i + 2]; //B
                }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData asfas = bmp.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(outputImage, 0, asfas.Scan0, outputImage.Length);
            bmp.UnlockBits(asfas);
            return bmp;
        }
    }
}
