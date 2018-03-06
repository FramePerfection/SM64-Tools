using System;
using System.Collections.Generic;
using System.Text;

namespace SM64RAM
{
    public static class Utility
    {
        public static void SetBit(ref int value, int position, bool bit)
        {
            value = (value & ~(1 << position));
            if (bit) value |= (1 << position);
        }
        public static bool GetBit(int value, int position)
        {
            return (value & (1 << position)) != 0;
        }
        public static void SetBits(ref int value, int n, int shift, int bits)
        {
            int mask = ((1 << n) - 1) << shift;
            value = value & ~mask | ((bits << shift) & mask);
        }
        public static int GetBits(int value, int n, int shift)
        {
            return (value >> shift) & ((1 << n) - 1);
        }


        public static void SetBit(ref long value, int position, bool bit)
        {
            value = (value & ~(1 << position));
            if (bit) value |= (1 << position);
        }
        public static bool GetBit(long value, int position)
        {
            return (value & (1 << position)) != 0;
        }
        public static void SetBits(ref long value, int n, int shift, long bits)
        {
            long mask = ((long)(1 << n) - 1) << shift;
            value = value & ~mask | ((bits << shift) & mask);
        }
        public static long GetBits(long value, int n, int shift)
        {
            return (value >> shift) & ((1 << n) - 1);
        }

        public static float Fixed2Float(int fixedPointValue, int postCommaDigits)
        {
            return (float)((double)fixedPointValue / (1 << postCommaDigits));
        }
    }
}
