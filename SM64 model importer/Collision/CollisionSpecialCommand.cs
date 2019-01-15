using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SM64RAM;

namespace SM64ModelImporter
{
    public class SpecialCollisionBox
    {
        public short type, x1, z1, x2, z2, y;
    }

    public class SpecialBoxes
    {
        public List<SpecialCollisionBox> boxes = new List<SpecialCollisionBox>();
        public void Save(ref byte[] stream, ref int cursor)
        {
            int length = GetLength();
            if (stream.Length < cursor + length)
                Array.Resize(ref stream, cursor + length);
            cvt.writeInt16(stream, cursor, 0x44);
            cvt.writeInt16(stream, cursor + 2, boxes.Count);
            cursor += 4;
            foreach (SpecialCollisionBox box in boxes)
            {
                cvt.writeInt16(stream, cursor, box.type);
                cvt.writeInt16(stream, cursor + 2, box.x1);
                cvt.writeInt16(stream, cursor + 4, box.z1);
                cvt.writeInt16(stream, cursor + 6, box.x2);
                cvt.writeInt16(stream, cursor + 8, box.z2);
                cvt.writeInt16(stream, cursor + 0xA, box.y);
                cursor += 0xC;
            }
        }
        public int GetLength()
        {
            return boxes.Count * 0xC + 4;
        }
    }
}
