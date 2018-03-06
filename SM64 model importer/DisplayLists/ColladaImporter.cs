using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace SM64_model_importer
{
    class ColladaImporter
    {
        public static void Read(string fileName, ref DisplayList dsp, out Dictionary<string, TextureInfo> allMaterials, out string[] messages)
        {
            System.Xml.XmlDataDocument doc = new XmlDataDocument();
            doc.Load(fileName);
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("df", doc.DocumentElement.NamespaceURI);
            XmlNodeList meshNodes = doc.SelectNodes("//df:COLLADA/df:library_geometries/df:geometry/df:mesh", mgr);
            foreach (XmlNode mesh in meshNodes)
            {
                XmlNodeList sourceNodes = mesh.SelectNodes("df:source", mgr);
                foreach (XmlNode source in sourceNodes)
                {
                    float[] sourceArray = Array.ConvertAll(source.InnerText.Split(' '), (str) => { return float.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture); }); ;
                    object a = source.Attributes["count"];
                    if (sourceArray.Length != int.Parse(source.Attributes["count"].Value))
                        throw new Exception("Invalid source array value");
                }
            }

            dsp = null;
            allMaterials = null;
            messages = new string[] { "Not implemented!" };
            throw new NotImplementedException();
        }
    }
}
