using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using SM64RAM;

namespace SM64LevelEditor
{
    public class BankDescription
    {
        public enum BankType : byte
        {
            Undefined = 0xFF,
            Background = 0xA
        }
        public byte ID;
        public BankType type { get { return (BankType)ID; } set { ID = (byte)value; } }
        public uint ROM_Start, ROM_End;
        public string name;
        public bool compressed;
        public byte arg;

        public static BankDescription Parse(string str)
        {
            BankDescription output = new BankDescription();
            string[] split = str.Split(new string[] { ":=" }, StringSplitOptions.None);
            if (split.Length != 2) return null;
            string[] rangeSplit = split[1].Split('-');
            if (rangeSplit.Length != 2) return null;
            string[] typeSplit = rangeSplit[1].Split('(');
            if (typeSplit.Length == 2)
            {
                string argString = typeSplit[1].Trim();
                if (argString.EndsWith(")"))
                {
                    argString = argString.Substring(0, argString.Length - 1);
                    string[] args = argString.Split(',');
                    if (args.Length != 2) return null;
                    string typeString = args[0].Trim().ToLower();
                    if (typeString.StartsWith("0x"))
                    {
                        if (!byte.TryParse(typeString.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out output.ID))
                            return null;
                    }
                    else
                    {
                        object enumEntry = Enum.Parse(typeof(BankType), typeString, true);
                        if (enumEntry == null) return null;
                        else output.ID = (byte)enumEntry;
                    }
                    string compString = args[1].Trim().ToLower();
                    if (compString == "compressed")
                        output.compressed = true;
                    else if (compString == "uncompressed")
                        output.compressed = false;
                    else
                        return null;
                }
            }
            if (!uint.TryParse(typeSplit[0].Trim().Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out output.ROM_End))
                return null;
            if (!uint.TryParse(rangeSplit[0].Trim().Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out output.ROM_Start))
                return null;
            output.name = split[0];
            return output;
        }

        public static BankDescription Parse(byte[] commandBytes)
        {
            if (commandBytes.Length < 0xC || commandBytes.Length != commandBytes[1]) throw new ArgumentException("Invalid command bytes!");
            BankDescription output = new BankDescription();
            output.compressed = commandBytes[0] != 0x17;
            output.arg = commandBytes[2];
            output.ID = commandBytes[3];
            output.ROM_Start = cvt.uint32(commandBytes, 4);
            output.ROM_End = cvt.uint32(commandBytes, 8);
            return Editor.projectSettings.UpdateBank(output);
        }
    }
}
