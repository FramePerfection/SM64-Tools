using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;
using System.Globalization;

namespace SM64LevelEditor
{
    public class Warp
    {
        public byte sourceID;
        public byte destinationLevel;
        public byte destinationArea;
        public byte destinationID;

        public Warp(byte sourceID, byte destinationLevel, byte destinationArea, byte destinationID)
        {
            this.sourceID = sourceID;
            this.destinationLevel = destinationLevel;
            this.destinationArea = destinationArea;
            this.destinationID = destinationID;
        }

        public static Warp FromString(string source)
        {
            string[] split = source.Split(new string[] {"=>"}, StringSplitOptions.None);
            if (split.Length != 2) return null;
            string[] destSplit = split[1].Split(',');
            if (destSplit.Length != 3) return null;
            byte sourceID, destinationLevel, destinationArea, destinationID;
            if (!(byte.TryParse(split[0].Trim(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out sourceID) &&
                byte.TryParse(destSplit[0].Trim(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out destinationLevel) &&
                byte.TryParse(destSplit[1].Trim(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out destinationArea) &&
                byte.TryParse(destSplit[2].Trim(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out destinationID))) return null;
            return new Warp(sourceID, destinationLevel, destinationArea, destinationID);
        }
    }
}
