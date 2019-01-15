using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace SM64RAM
{
    public class cvt
    {
        public static int int32(byte[] bytes, int offset)
        {
            return bytes[offset] << 0x18 | bytes[offset + 1] << 0x10 | bytes[offset + 2] << 0x8 | bytes[offset + 3];
        }

        public static uint uint32(byte[] bytes, int offset)
        {
            try
            {

                return (uint)(bytes[offset] << 0x18 | bytes[offset + 1] << 0x10 | bytes[offset + 2] << 0x8 | bytes[offset + 3]);
            }
            catch {
                ;
            }
            return 0;
        }

        public static short int16(byte[] bytes, int offset)
        {
            return (short)(bytes[offset] << 0x8 | bytes[offset + 1]);
        }

        public static void writeInt32(byte[] bytes, int offset, int value)
        {
            bytes[offset + 3] = (byte)(value & 0xFF);
            bytes[offset + 2] = (byte)((value & 0xFF00) >> 8);
            bytes[offset + 1] = (byte)((value & 0xFF0000) >> 0x10);
            bytes[offset] = (byte)((value & 0xFF000000) >> 0x18);
        }

        public static void writeInt16(byte[] bytes, int offset, int value)
        {
            bytes[offset + 1] = (byte)(value & 0xFF);
            bytes[offset + 0] = (byte)((value & 0xFF00) >> 8);
        }

        public static bool ParseIntHex(string input, out int result)
        {
            return int.TryParse(input, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result);
        }


        public static Vector3 ParseVector3(string[] inputs, int offset)
        {
            float x, y, z;
            if (!float.TryParse(inputs[offset], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out x) ||
                !float.TryParse(inputs[offset + 1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out y) ||
                !float.TryParse(inputs[offset + 2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out z))
                return new Vector3(0, 0, 0);
            return new Vector3(x, y, z);
        }
        public static Vector2 parseVector2(string[] inputs, int offset)
        {
            float x, y;
            if (!float.TryParse(inputs[offset], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out x) ||
                !float.TryParse(inputs[offset + 1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out y))
                return new Vector2(0, 0);
            return new Vector2(x, y);
        }
    }
}
