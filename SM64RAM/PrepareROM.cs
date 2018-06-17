using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SM64RAM
{
    public static class PrepareROM
    {
        public enum Endianness
        {
            Little_Endian,
            Mixed_Endian,
            Big_Endian,
            Unknown,
            Invalid
        }

        public const int REGULAR_ROM_SIZE = 0x800000;
        public const int SUGGESTED_ROM_SIZE = 0x3000000;
        const string EXPAND_ROM_MESSAGE = "Loaded ROM is not 64MB. To use the presets that come with this tool, it is suggested that you expand your ROM to 64MB\nAll this will do is increase the file size and add 01 padding in the new areas. Do you want to do that now?";
        public static byte[] Normalize(string inputFile)
        {
            byte[] inputBytes = File.ReadAllBytes(inputFile);

            Endianness endian = GetEndianness(inputBytes);
            if (endian == Endianness.Mixed_Endian)
            { MixedEndianToLittleEndian(ref inputBytes); endian = Endianness.Little_Endian; }
            if (endian == Endianness.Big_Endian)
            { BigEndianToLittleEndian(ref inputBytes); endian = Endianness.Little_Endian; }

            if (inputBytes.Length <= REGULAR_ROM_SIZE)
            {
                if (MessageBox.Show(EXPAND_ROM_MESSAGE, "Expand ROM?", MessageBoxButtons.YesNo) != DialogResult.Yes) return inputBytes;
                int initialSize = inputBytes.Length;
                Array.Resize(ref inputBytes, SUGGESTED_ROM_SIZE);
                for (int i = initialSize; i < SUGGESTED_ROM_SIZE; i++)
                    inputBytes[i] = 0x01;
                File.WriteAllBytes(inputFile, inputBytes);
            }
            return inputBytes;
        }

        static Endianness GetEndianness(byte[] file)
        {
            if (file[0x3B] == 0x4E && file[0x3C] == 0x53 && file[0x3D] == 0x4D && file[0x3E] == 0x45) return Endianness.Little_Endian;
            if (file[0x38] == 0x4E && file[0x3F] == 0x53 && file[0x3E] == 0x4D && file[0x3B] == 0x45) return Endianness.Big_Endian;
            if (file[0x3A] == 0x4E && file[0x3D] == 0x53 && file[0x3C] == 0x4D && file[0x3F] == 0x45) return Endianness.Mixed_Endian;
            return Endianness.Invalid;
        }

        static void MixedEndianToLittleEndian(ref byte[] bytes)
        {
            if (bytes.Length % 2 != 0)
                Array.Resize(ref bytes, bytes.Length + 1);
            for (int i = 0; i < bytes.Length; i += 2)
            {
                byte tmp = bytes[i];
                bytes[i] = bytes[i + 1];
                bytes[i + 1] = tmp;
            }
        }

        static void BigEndianToLittleEndian(ref byte[] bytes)
        {
            if (bytes.Length % 4 != 0)
                Array.Resize(ref bytes, (bytes.Length + 3) / 4 * 4);
            for (int i = 0; i < bytes.Length; i += 4)
            {
                byte tmp = bytes[i];
                bytes[i] = bytes[i + 3];
                bytes[i + 3] = tmp;
                tmp = bytes[i + 1];
                bytes[i + 1] = bytes[i + 2];
                bytes[i + 2] = tmp;
            }
        }
    }
}
