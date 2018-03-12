using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SM64RAM;

namespace SM64ModelImporter
{
    public class Vertex
    {
        public int x, y, z;
        public double u, v;
        public sbyte nx, ny, nz;
        public byte c = 255;

        public Vertex(Vector3 position, Vector2 TexCoord, Vector3 Normal)
        {
            x = (short)(position.X * Settings.importScale);
            y = (short)(position.Y * Settings.importScale);
            z = (short)(position.Z * Settings.importScale);

            u = TexCoord.X;
            v = TexCoord.Y;
            Normal.Normalize();
            nx = (sbyte)(Normal.X * sbyte.MaxValue);
            ny = (sbyte)(Normal.Y * sbyte.MaxValue);
            nz = (sbyte)(Normal.Z * sbyte.MaxValue);
        }
    }

    public struct Triangle
    {
        public int v1, v2, v3;
        public Triangle(int v1, int v2, int v3)
        {
            this.v1 = v1; this.v2 = v2; this.v3 = v3;
        }
    }

    public class TrianglePatch
    {
        public int segmentedPointer = 0;
        public List<Triangle> triangles = new List<Triangle>();
        public List<Vertex> vertices = new List<Vertex>();
        public int numVertices = 0;
        public void crop()
        {
            int maxIndex = 0;
            foreach (Triangle t in triangles)
                maxIndex = Math.Max(Math.Max(Math.Max(maxIndex, t.v1), t.v2), t.v3);
            numVertices = maxIndex + 1;
        }

        public int GetSizeInBytes()
        {
            return numVertices * 0x10;
        }

        public void WriteBytes(int texMulX, int texMulY)
        {
            int segment = segmentedPointer >> 0x18;
            EmulationState.RAMBank target = EmulationState.instance.banks[segment];
            if (target == null)
                throw new Exception("Bad Vertex Data");

            int i = 0;
            int cursor = segmentedPointer & 0xFFFFFF;
            byte[] bytes = target.value;
            foreach (Vertex v in vertices)
            {
                byte[] b = BitConverter.GetBytes(v.x);
                bytes[cursor + 1] = b[0]; bytes[cursor + 0] = b[1];
                b = BitConverter.GetBytes(v.y);
                bytes[cursor + 3] = b[0]; bytes[cursor + 2] = b[1];
                b = BitConverter.GetBytes(v.z);
                bytes[cursor + 5] = b[0]; bytes[cursor + 4] = b[1];
                b = BitConverter.GetBytes((short)(v.u * texMulX * (1 << 5)));
                bytes[cursor + 9] = b[0]; bytes[cursor + 8] = b[1];
                b = BitConverter.GetBytes((short)(v.v * texMulY * (1 << 5)));
                bytes[cursor + 0xB] = b[0]; bytes[cursor + 0xA] = b[1];
                bytes[cursor + 0xC] = (byte)v.nx;
                bytes[cursor + 0xD] = (byte)v.ny;
                bytes[cursor + 0xE] = (byte)v.nz;
                bytes[cursor + 0xF] = v.c;
                cursor += 0x10;
                if (i++ >= numVertices) break;
            }
        }
    }
}
