using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SM64RAM;

namespace SM64ModelImporter
{
    public interface ReadWrite
    {
        void SaveSettings(FileParser.Block writer);
        void LoadSettings(FileParser.Block reader);
    }

    public class FileParser
    {

        public class Block
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            Dictionary<string, Block> blocks = new Dictionary<string, Block>();
            public string fileName { get; private set; }

            public Block(ReadWrite obj)
            {
                obj.SaveSettings(this);
            }

            public Block(string fileName) {
                this.fileName = fileName;
                StreamReader reader = new StreamReader(fileName); 
                Read(reader, true); 
                reader.Close();
            }

            private Block(StreamReader reader, string fileName) {
                this.fileName = fileName;
                Read(reader, false); 
            }

            private void Read(StreamReader reader, bool isRoot)
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    if (line == "") continue;
                    if (line == "}")
                        return;
                    string[] split = line.Split(new string[] { ":=" }, 2, StringSplitOptions.None);
                    split[1] = split[1].Trim();
                    if (split.Length == 2)
                    {
                        if (split[1] == "{")
                            blocks[split[0].Trim()] = new Block(reader, fileName);
                        else
                            values[split[0].Trim()] = split[1];
                    }
                    else
                        throw new Exception("Invalid statement.");
                }
                if (!isRoot)
                    throw new Exception("Unexpected end of stream");
            }

            public void Write(StreamWriter writer, int numTabs = 0)
            {
                foreach (KeyValuePair<string, string> a in values)
                    WriteTabLine(writer, numTabs, a.Key + ":= " + a.Value);
                foreach (KeyValuePair<string, Block> a in blocks)
                {
                    WriteTabLine(writer, numTabs, a.Key + ":= {");
                    a.Value.Write(writer, numTabs + 1);
                    WriteTabLine(writer, numTabs, "}");
                }
            }

            static void WriteTabLine(StreamWriter writer, int numTabs, string text)
            {
                for (int i = 0; i < numTabs; i++)
                    writer.Write("\t");
                writer.WriteLine(text);
            }

            public bool GetBool(string name, bool throwException = true)
            {
                bool b = false;
                string value;
                if (!values.TryGetValue(name.Trim(), out value))
                {
                    if (throwException)
                        throw new InvalidOperationException("Parameter " + name + " was not found in block.");
                }
                else if (!bool.TryParse(value, out b) && throwException)
                    throw new InvalidOperationException("Parameter " + name + " cannot be treaded as a bool.");
                return b;
            }

            public int GetInt(string name, bool throwException = true)
            {
                int i = 0;
                string value;
                if (!values.TryGetValue(name.Trim(), out value))
                {
                    if (throwException)
                        throw new InvalidOperationException("Parameter " + name + " was not found in block.");
                }
                else
                {
                    if (value.StartsWith("0x"))
                    {
                        if (!cvt.ParseIntHex(value.Remove(0, 2), out i))
                            throw new InvalidOperationException("Invalid value for parameter " + name + "(" + value + ")");
                    }
                    else if (!int.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out i) && throwException)
                        throw new InvalidOperationException("Invalid value for parameter " + name + "(" + value + ")");
                }
                return i;
            }

            public double GetDouble(string name, bool throwException = true)
            {
                double i = 0;
                string value;
                if (!values.TryGetValue(name.Trim(), out value))
                {
                    if (throwException)
                        throw new InvalidOperationException("Parameter " + name + " was not found in block.");
                }
                else if (!double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out i) && throwException)
                    throw new InvalidOperationException("Invalid value for parameter " + name + "(" + value + ")");
                return i;
            }

            public string GetString(string name, bool throwException = true)
            {
                string value;
                if (!values.TryGetValue(name.Trim(), out value))
                {
                    if (throwException)
                        throw new InvalidOperationException("Parameter " + name + " was not found in block.");
                }
                else
                {
                    if ((value.Length == 0 || (value[0] != '\"' || value[value.Length - 1] != '\"')) && throwException)
                        throw new InvalidOperationException("Invalid value for parameter " + name + "(" + value + ")");
                    else
                        return value.Remove(0, 1).Remove(value.Length - 2);
                }
                return "";
            }

            public int[] GetIntArray(string name, bool throwException = true)
            {
                string value;
                if (!values.TryGetValue(name.Trim(), out value))
                {
                    if (throwException)
                        throw new InvalidOperationException("Parameter " + name + " was not found in block.");
                }
                else
                {
                    string[] split = value.Split(',');
                    if (split.Length == 1 && (split[0].Trim() == "")) return new int[] { };
                    int[] output = new int[split.Length];
                    for (int i = 0; i < output.Length; i++)
                    {
                        string trim = split[i].Trim();
                        if (trim.StartsWith("0x"))
                        {
                            if (!cvt.ParseIntHex(trim.Remove(0, 2), out output[i]) && throwException)
                                throw new Exception("Invalid value in " + name + " (" + trim + ")");
                        }
                        else if (!int.TryParse(trim, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out output[i]))
                            throw new Exception("Invalid value in " + name + " (" + trim + ")");
                    }
                    return output;
                }
                return new int[] { };
            }

            public Block GetBlock(string name, bool throwException = true)
            {
                Block output;
                if (!blocks.TryGetValue(name.Trim(), out output) && throwException)
                    throw new InvalidOperationException("Parameter " + name + " was not found in block.");
                return output;
            }

            public void SetBool(string name, bool value)
            {
                values[name.Trim()] = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public void SetInt(string name, int value, bool hex = true)
            {
                if (hex)
                    values[name.Trim()] = "0x" + value.ToString("X");
                else
                    values[name.Trim()] = value.ToString();
            }

            public void SetDouble(string name, double value)
            {
                values[name.Trim()] = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public void SetString(string name, string value)
            {
                values[name.Trim()] = "\"" + value.ToString() + "\"";
            }

            public void SetIntArray(string name, int[] value, bool hex = true)
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < value.Length; i++)
                {
                    b.Append(hex ? ("0x" + value[i].ToString("X")) : value[i].ToString());
                    if (i < value.Length - 1) b.Append(", ");
                }
                values[name.Trim()] = b.ToString();
            }

            public void SetBlock(string name, Block block)
            {
                blocks[name.Trim()] = block;
            }
        }
    }
}
