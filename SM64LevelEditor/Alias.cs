using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SM64LevelEditor
{
    public struct Alias<T>
    {
        public string alias;
        public T value;
        public delegate T FromString(string input);
        public delegate string ToStringFunc(T input);
        ToStringFunc toString;

        public static List<Alias<T>> LoadFile(string fileName, FromString fromString, ToStringFunc toString)
        {
            List<Alias<T>> newList = new List<Alias<T>>();
            StreamReader rd = new StreamReader(fileName);
            while (!rd.EndOfStream)
            {
                string line = rd.ReadLine().Trim();
                if (line == "") continue;
                string[] split = line.Split(new string[] { ":=" }, 2, StringSplitOptions.None);
                if (split.Length != 2) continue;
                newList.Add(new Alias<T>(split[0].Trim(), fromString(split[1].Trim()), toString));
            }
            rd.Close();
            return newList;
        }

        public Alias(string alias, T value, ToStringFunc toString) { this.alias = alias; this.value = value; this.toString = toString; }

        public override string ToString()
        {
            return alias + ": " + toString(value);
        }
    }
}
