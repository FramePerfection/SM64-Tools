using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace SM64ModelImporter.F3DParser
{
    public enum ArgumentType
    {
        Numeric,
        Color,
        Pointer,
        Other
    }

    public class Argument
    {
        ulong value;
        PointerReference identifier;
        string argString;
        bool parsed = false;
        ArgumentType type;
        string argName;
        F3DParser parser;
        public Argument(F3DParser parser, ArgumentType type, string argName, string argString)
        {
            this.parser = parser;
            this.type = type;
            this.argName = argName;
            this.argString = argString = argString.Trim();
        }

        public void Parse()
        {
            if (parsed) return;
            if (argString.Length == 0)
                throw new F3DParserException("No argument provided for argument " + argName + ".");
            switch (type)
            {
                #region Numeric
                case ArgumentType.Numeric:
                    string[] subArgs = argString.Split('|');
                    foreach (string subArg in subArgs)
                    {
                        string arg = subArg.Trim();
                        ulong newValue;
                        if (F3DCommandDescriptors.enumValues.TryGetValue(arg, out newValue))
                        {
                            value |= newValue;
                            goto Done;
                        }
                        else if (arg.ToLower().StartsWith("0x")) //Hexadecimal integer
                        {
                            if (!ulong.TryParse(argString.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out newValue))
                                throw new Exception("Invalid hexadecimal numeric value for argument " + argName + ".");
                            value |= newValue;
                            goto Done;
                        }
                    }
                    if (subArgs.Length == 1)
                    {
                        argString = argString.ToLower();
                        if (argString.EndsWith("f")) // floating point value
                        {
                            float v;
                            if (!float.TryParse(argString.Remove(argString.Length - 1), NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                                throw new Exception("Invalid floating point value for argument " + argName + ".");
                            value = BitConverter.ToUInt32(BitConverter.GetBytes(v), 0);
                        }
                        else
                        {
                            int decimalPointPosition = argString.IndexOf('.');
                            if (decimalPointPosition == -1) //Decimal integer
                            {
                                if (!ulong.TryParse(argString, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                                    throw new Exception("Invalid decimal numeric value for argument " + argName + ".");
                            }
                            else
                            {
                                if (!ulong.TryParse(argString.Substring(0, decimalPointPosition) + argString.Substring(decimalPointPosition + 1), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                                    throw new Exception("Invalid fixed point numeric value for argument " + argName + ".");
                            }
                        }
                    }
                Done:
                    break;
                #endregion
                #region Pointer
                case ArgumentType.Pointer:
                    if (parser.knownReferences.TryGetValue(argString, out identifier)) //Pointer by identifier
                        argString = argString;
                    else
                    {
                        argString = argString.ToLower();
                        if (argString.StartsWith("0x")) //Immediate Hexadecimal Pointer
                        {
                            if (!ulong.TryParse(argString.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out value))
                                throw new Exception("Invalid hexadecimal pointer value for argument " + argName + ".");
                        }
                        else
                            throw new Exception("Invalid pointer value '" + argString + "' for argument " + argName + ".");
                    }
                    break;
                #endregion
            }
            parsed = true;
        }

        public ulong GetValue()
        {
            Parse();
            if (identifier != null)
            {
                if (!identifier.resolved)
                    throw new F3DParserException("Identifier " + argString + " is not resolved.");
                return (ulong)identifier.pointer;
            }
            else
                return value;
        }

        public static implicit operator ulong(Argument arg)
        {
            return arg.GetValue();
        }

        public static implicit operator int(Argument arg)
        {
            return (int)arg.GetValue();
        }

        public static implicit operator short(Argument arg)
        {
            return (short)arg.GetValue();
        }

        public static implicit operator byte(Argument arg)
        {
            return (byte)arg.GetValue();
        }
    }
}
