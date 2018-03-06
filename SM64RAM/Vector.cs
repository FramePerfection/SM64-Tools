using System;
using System.Collections.Generic;
using System.Text;

namespace SM64RAM
{
    public struct Vector3
    {
        public float X, Y, Z;
        public Vector3(float X, float Y, float Z) { this.X = X; this.Y = Y; this.Z = Z; }
        public static Vector3 Empty { get { return new Vector3(); } }
        public static Vector3 Normalize(Vector3 input)
        {
            float len = input.Length();
            return new Vector3(input.X / len, input.Y / len, input.Z / len);
        }
        public float Length() { return (float)Math.Sqrt(LengthSq()); }
        public float LengthSq() { return X * X + Y * Y + Z * Z; }
        public void Normalize()
        {
            float len = Length();
            X /= len; Y /= len; Z /= len;
        }

        public static Vector3 operator *(Vector3 v, float f)
        {
            return new Vector3(v.X * f, v.Y * f, v.Z * f);
        }
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return (a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return (a.X != b.X || a.Y != b.Y || a.Z != b.Z);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public struct Vector2
    {
        public float X, Y;
        public Vector2(float X, float Y) { this.X = X; this.Y = Y; }
        public static Vector2 Empty { get { return new Vector2(); } }
    }
}
