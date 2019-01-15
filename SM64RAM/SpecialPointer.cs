using System;
using System.Collections.Generic;
using System.Text;

namespace SM64RAM
{
    public abstract class SpecialPointer
    {
        public abstract int GetSegmentedValue();
        private string typeString;
        public SpecialPointer()
        {
            typeString = GetType().Name;
            if (typeString.EndsWith("Pointer"))
                typeString = typeString.Substring(0, typeString.Length - "Pointer".Length);
        }
        public override string ToString() {
            return typeString + ": " + Identifier();
        }
        protected abstract string Identifier();
    }

    public class AreaCollisionPointer : SpecialPointer
    {
        public readonly int area = 0;
        public readonly int segmentedAddress = 0;
        public AreaCollisionPointer(int area, int segmentedAddress)
        {
            this.area = area;
            this.segmentedAddress = segmentedAddress;
        }
        public override int GetSegmentedValue()
        {
            return segmentedAddress + 4;
        }

        protected override string Identifier()
        {
            return "Area " + area.ToString();
        }
    }
}
