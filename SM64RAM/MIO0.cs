using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SM64RAM
{
    public class MIO0
    {

        public static byte[] decode_MIO0(byte[] ROM, int offset)
        {
            long baseOffset = offset;

            int bytesWritten = 0, destSize = 0, comp_offset, uncomp_offset;
            int comp_position = 0, uncomp_position = 0;
            int bit = 0;
            int bitsBaseOffset = offset + 0x10;

            byte[] outputStream = null;

            if (System.Text.ASCIIEncoding.ASCII.GetString(ROM, offset, 4) == "MIO0")
            {
                destSize = cvt.int32(ROM, offset + 4);
                comp_offset = cvt.int32(ROM, offset + 8);
                uncomp_offset = cvt.int32(ROM, offset + 0xC);

                comp_position = offset + comp_offset;
                uncomp_position = offset + uncomp_offset;
                outputStream = new byte[destSize];
            }
            else
            {
                MessageBox.Show("Invalid MIO0 header.");
                return null;
            }

            while (bytesWritten < destSize)
            {
                if ((ROM[offset + 0x10 + bit / 8] & (0x80 >> (bit % 8))) > 0)
                { //uncompressed data
                    outputStream[bytesWritten] = ROM[uncomp_position];
                    uncomp_position++;
                    bytesWritten++;
                }
                else
                { //compressed data
                    int length = ((ROM[comp_position] & 0xF0) >> 4) + 3;
                    int position = (((ROM[comp_position] & 0x0F) << 0x8) | ROM[comp_position + 1]) + 1;
                    for (int i = 0; i < length; i++)
                    {
                        outputStream[bytesWritten] = outputStream[bytesWritten - position];
                        bytesWritten++;
                    }
                    comp_position += 2;
                }
                bit++;
            }
            return outputStream;
        }

        public static byte[] encode_MIO0(byte[] input)
        {
            byte[] bits = new byte[input.Length / 8];
            byte[] uncompressed = new byte[input.Length];
            byte[] compressed = new byte[input.Length];
            int bitIndex = 1;
            int uncompIndex = 1;
            int compIndex = 0;

            int bytesProcessed = 1;
            put_bit(ref bits, 0, true);
            uncompressed[0] = input[0];
            while (bytesProcessed < input.Length)
            {
                int offset;
                int max_length = Math.Min(input.Length - bytesProcessed, 18);
                int longest_match = find_longest(input, bytesProcessed, max_length, out offset);
                if (longest_match > 2)
                {
                    int lookahead_offset;
                    // lookahead to next byte to see if longer match
                    int lookahead_length = Math.Min(input.Length - bytesProcessed - 1, 18);
                    int lookahead_match = find_longest(input, bytesProcessed + 1, lookahead_length, out lookahead_offset);
                    // better match found, use uncompressed + lookahead compressed
                    if ((longest_match + 1) < lookahead_match)
                    {
                        // uncompressed byte
                        uncompressed[uncompIndex] = input[bytesProcessed];
                        uncompIndex++;
                        put_bit(ref bits, bitIndex, true);
                        bytesProcessed++;
                        longest_match = lookahead_match;
                        offset = lookahead_offset;
                        bitIndex++;
                    }
                    // compressed block
                    compressed[compIndex] = (byte)((((longest_match - 3) & 0x0F) << 4) |
                                         (((offset - 1) >> 8) & 0x0F));
                    compressed[compIndex + 1] = (byte)((offset - 1) & 0xFF);
                    compIndex += 2;
                    put_bit(ref bits, bitIndex, false);
                    bytesProcessed += longest_match;
                }
                else
                {
                    put_bit(ref bits, bitIndex, true);
                    uncompressed[uncompIndex] = input[bytesProcessed];
                    uncompIndex++;
                    bytesProcessed++;
                }
                bitIndex++;
            }
            int bitBytes = (bitIndex + 7) / 8;
            int compressedOffset = bitBytes + 0x10;
            compressedOffset = ((compressedOffset + 3) / 4) * 4;
            int uncompressedOffset = compressedOffset + compIndex;

            byte[] output = new byte[uncompressedOffset + uncompIndex];
            output[0] = (byte)'M'; output[1] = (byte)'I'; output[2] = (byte)'O'; output[3] = (byte)'0'; //MIO0
            cvt.writeInt32(output, 4, input.Length);
            cvt.writeInt32(output, 8, compressedOffset);
            cvt.writeInt32(output, 12, uncompressedOffset);
            Array.Copy(bits, 0, output, 0x10, bitBytes);
            Array.Copy(compressed, 0, output, compressedOffset, compIndex);
            Array.Copy(uncompressed, 0, output, uncompressedOffset, uncompIndex);
            return output;
        }

        private static void put_bit(ref byte[] bits, int index, bool value)
        {
            byte mask = (byte)(0x80 >> (index % 8));
            if (value) bits[index / 8] |= mask;
            else bits[index / 8] &= (byte)~mask;
        }

        private static int find_longest(byte[] buf, int start_offset, int max_search, out int found_offset)
        {
            int best_length = 0;
            int best_offset = 0;
            int cur_length;
            int search_len;
            int off, i;

            // check at most the past 4096 values
            for (off = Math.Max(start_offset - 4096, 0); off < start_offset; off++)
            {
                // check at most requested max or up until start
                search_len = Math.Min(max_search, start_offset - off);
                for (i = 0; i < search_len; i++)
                {
                    if (buf[start_offset + i] != buf[off + i])
                    {
                        break;
                    }
                }
                cur_length = i;
                // if matched up until start, continue matching in already matched parts
                if (cur_length == search_len)
                {
                    // check at most requested max less current length
                    search_len = max_search - cur_length;
                    for (i = 0; i < search_len; i++)
                    {
                        if (buf[start_offset + cur_length + i] != buf[off + i])
                        {
                            break;
                        }
                    }
                    cur_length += i;
                }
                if (cur_length > best_length)
                {
                    best_offset = start_offset - off;
                    best_length = cur_length;
                }
            }

            // return best reverse offset and length (may be 0)
            found_offset = best_offset;
            return best_length;
        }
    }
}
