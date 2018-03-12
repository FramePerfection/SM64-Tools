using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;
using SM64RAM;
using System.Reflection;

namespace SM64ModelImporter
{
   public  class ColladaImporter
    {

       //ignore warnings realted to these variables being unassigned. They will be set via reflection.
        struct Color { public float R, G, B, A;}
        struct TexCoord { public float S, T;}
        struct Vec3 { public float X, Y, Z;}

        public static void Read(string fileName, ConversionSettings conversionSettings, ref DisplayList dsp, out Dictionary<string, TextureInfo> allMaterials, out string[] messages)
        {
            messages = new string[] { "Collada importing is not well tested nor fully implemented yet!" };
            if (conversionSettings.colorInterpretation == ConversionSettings.ColorInterpretation.Undefined)
                if (!conversionSettings.DoColorInterpretationDialog())
                {
                    allMaterials = null;
                    return;
                }
            string rootDirectory = System.IO.Path.GetDirectoryName(fileName);

            System.Xml.XmlDataDocument doc = new XmlDataDocument();
            doc.Load(fileName);
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("df", doc.DocumentElement.NamespaceURI);
            XmlNodeList meshNodes = doc.SelectNodes("//df:COLLADA/df:library_geometries/df:geometry/df:mesh", mgr);
            List<Subset> meshes = new List<Subset>();
            XmlNode imageLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_images", mgr);
            XmlNode materialLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_materials", mgr);
            XmlNode effectLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_effects", mgr);
            bool flipYZ = doc.SelectSingleNode("//df:COLLADA/df:asset/df:up_axis", mgr).InnerText == "Z_UP";

            allMaterials = new Dictionary<string, TextureInfo>();
            allMaterials["<Undefined>"] = new TextureInfo(null);

            foreach (XmlNode meshNode in meshNodes)
            {
                Subset meshSubset = new Subset();
                meshes.Add(meshSubset);
                foreach (XmlNode trianglesNode in meshNode.SelectNodes("df:triangles", mgr))
                {
                    List<Vertex> vertexBuffer = new List<Vertex>(); //final vertex buffer for this material
                    List<int> indexBuffer = new List<int>(); //final index buffer for this material

                    //Data to be read from XML
                    string material = trianglesNode.Attributes["material"].Value;
                    string effect = materialLibraryNode.SelectSingleNode("df:material[@id='" + material + "']/df:instance_effect", mgr).Attributes["url"].Value.Remove(0, 1);
                    XmlNode effectNode = effectLibraryNode.SelectSingleNode("df:effect[@id='" + effect + "']/df:profile_COMMON", mgr);
                    XmlNode textureNode = effectNode.SelectSingleNode("df:technique/*/df:diffuse/df:texture", mgr);
                    if (textureNode == null)
                        meshSubset.Texture = allMaterials["<Undefined>"];
                    else
                    {
                        string surfaceName = effectNode.SelectSingleNode("df:newparam[@sid='" + textureNode.Attributes["texture"].Value + "']/df:sampler2D/df:source", mgr).InnerText;
                        string init_from = effectNode.SelectSingleNode("df:newparam[@sid='" + surfaceName + "']/df:surface/df:init_from", mgr).InnerText;
                        string imageFileName = imageLibraryNode.SelectSingleNode("df:image[@id='" + init_from + "']/df:init_from", mgr).InnerText;
                        string fullPath = imageFileName;
                        if (imageFileName[1] != ':') //Path is not absolute. Make Absolute
                            fullPath = System.IO.Path.Combine(rootDirectory, imageFileName);
                        if (!allMaterials.TryGetValue(imageFileName, out meshSubset.Texture))
                            meshSubset.Texture = allMaterials[imageFileName] = new TextureInfo(fullPath);
                    }

                    int vertexOffset, normalOffset, texCoordOffset, colorOffset;
                    vertexOffset = normalOffset = texCoordOffset = colorOffset = -1;
                    Vec3[] positions = null, normals = null;
                    TexCoord[] texCoords = null;
                    Color[] colors = null;

                    XmlNode vertexNode = trianglesNode.SelectSingleNode("df:input[@semantic='VERTEX']", mgr);
                    if (vertexNode != null)
                    {
                        XmlNode vertexSource = meshNode.SelectSingleNode("df:vertices[@id='" + vertexNode.Attributes["source"].Value.Remove(0, 1) + "']", mgr);
                        XmlNode vertexSourceSource /*jeez pls give me vertex positions now*/ = meshNode.SelectSingleNode("df:source[@id='" + vertexSource.SelectSingleNode("df:input[@semantic='POSITION']", mgr).Attributes["source"].Value.Remove(0, 1) + "']", mgr);
                        positions = ReadXMLVectorNode<Vec3>(vertexSourceSource, mgr);
                        vertexOffset = int.Parse(vertexNode.Attributes["offset"].Value);
                    }

                    XmlNode normalNode = trianglesNode.SelectSingleNode("df:input[@semantic='NORMAL']", mgr);
                    if (normalNode != null)
                    {
                        XmlNode normalSource = meshNode.SelectSingleNode("df:source[@id='" + normalNode.Attributes["source"].Value.Remove(0, 1) + "']", mgr);
                        normals = ReadXMLVectorNode<Vec3>(normalSource, mgr);
                        normalOffset = int.Parse(normalNode.Attributes["offset"].Value);
                    }

                    XmlNode texCoordNode = trianglesNode.SelectSingleNode("df:input[@semantic='TEXCOORD']", mgr);
                    if (texCoordNode != null)
                    {
                        XmlNode texCoordSource = meshNode.SelectSingleNode("df:source[@id='" + texCoordNode.Attributes["source"].Value.Remove(0, 1) + "']", mgr);
                        texCoords = ReadXMLVectorNode<TexCoord>(texCoordSource, mgr);
                        texCoordOffset = int.Parse(texCoordNode.Attributes["offset"].Value);
                    }

                    XmlNode colorNode = trianglesNode.SelectSingleNode("df:input[@semantic='COLOR']", mgr);
                    if (colorNode != null)
                    {
                        XmlNode colorSource = meshNode.SelectSingleNode("df:source[@id='" + colorNode.Attributes["source"].Value.Remove(0, 1) + "']", mgr);
                        colors = ReadXMLVectorNode<Color>(colorSource, mgr);
                        colorOffset = int.Parse(colorNode.Attributes["offset"].Value);
                    }

                    int[] indices = Array.ConvertAll(trianglesNode.SelectSingleNode("df:p", mgr).InnerText.Split(' '), (string s) => int.Parse(s));
                    Vertex[] vertices = new Vertex[int.Parse(trianglesNode.Attributes["count"].Value) * 3];
                    int stride = indices.Length / vertices.Length;
                    int currentIndex = 0;
                    Color defaultColor = new Color();
                    defaultColor.R = defaultColor.G = defaultColor.B = defaultColor.A = 1;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        Vec3 position = positions == null ? new Vec3() : positions[indices[i * stride + vertexOffset]];
                        Vec3 normal = normals == null ? new Vec3() : normals[indices[i * stride + normalOffset]];
                        TexCoord texCoord = texCoords == null ? new TexCoord() : texCoords[indices[i * stride + texCoordOffset]];
                        Color color = colors == null ? defaultColor : colors[indices[i * stride + colorOffset]];
                        vertices[i] = flipYZ ? new Vertex(new Vector3(position.X, position.Z, -position.Y), new Vector2(texCoord.S, texCoord.T), new Vector3(normal.X, normal.Z, -normal.Y))
                            : new Vertex(new Vector3(position.X, position.Y, position.Z), new Vector2(texCoord.S, texCoord.T), new Vector3(normal.X, normal.Y, normal.Z));
                        Color c = colors[indices[i * stride + 3]];
                        if (conversionSettings.colorInterpretation == ConversionSettings.ColorInterpretation.ReplaceNormal)
                        {
                            vertices[i].nx = (sbyte)(c.R * 255);
                            vertices[i].ny = (sbyte)(c.G * 255);
                            vertices[i].nz = (sbyte)(c.B * 255);
                            vertices[i].c = 255;
                        }
                        else if (conversionSettings.colorInterpretation == ConversionSettings.ColorInterpretation.ConvertRedToAlpha)
                            vertices[i].c = (byte)(c.R * 255);
                        else if (conversionSettings.colorInterpretation == ConversionSettings.ColorInterpretation.ConvertGreenToAlpha)
                            vertices[i].c = (byte)(c.G * 255);
                        else if (conversionSettings.colorInterpretation == ConversionSettings.ColorInterpretation.ConvertBlueToAlpha)
                            vertices[i].c = (byte)(c.B * 255);
                        else if (conversionSettings.colorInterpretation == ConversionSettings.ColorInterpretation.ConvertRedToAlpha)
                            vertices[i].c = (byte)((c.R + c.G + c.B) / 3 * 255);

                        int k = 0;
                        foreach (Vertex v in vertexBuffer)
                        {
                            if (v.Equals(vertices[i]))
                            {
                                indexBuffer.Add(k);
                                goto skipNewVertex;
                            }
                            k++;
                        }
                        vertexBuffer.Add(vertices[i]);
                        indexBuffer.Add(currentIndex++);
                    skipNewVertex: ;
                    }
                    meshSubset.IndexBuffer = indexBuffer.ToArray();
                    meshSubset.VertexBuffer = vertexBuffer.ToArray();
                    meshSubset.CreatePatches();
                }
            }
            dsp.subsets = meshes.ToArray();
        }

        static T[] ReadXMLVectorNode<T>(XmlNode node, XmlNamespaceManager mgr) where T : struct
        {
            XmlNode accessorNode = node.SelectSingleNode("df:technique_common/df:accessor", mgr);
            XmlNodeList paramNodes = accessorNode.SelectNodes("df:param[@type='float']", mgr);
            XmlNode sourceArray = node.SelectSingleNode("df:float_array[@id='" + accessorNode.Attributes["source"].Value.Remove(0, 1) + "']", mgr);
            int stride = int.Parse(accessorNode.Attributes["stride"].Value);
            float[] sourceFloats = Array.ConvertAll(sourceArray.InnerText.Split(' '), (string s) => float.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
            T[] output = new T[int.Parse(accessorNode.Attributes["count"].Value)];
            FieldInfo[] matchingFields = new FieldInfo[paramNodes.Count];
            int i = 0;
            foreach (XmlNode paramNode in paramNodes)
                matchingFields[i++] = typeof(T).GetField(paramNode.Attributes["name"].Value);
            for (i = 0; i < output.Length; i++)
            {
                object newValue = default(T);
                for (int attr = 0; attr < matchingFields.Length; attr++)
                    matchingFields[attr].SetValue(newValue, sourceFloats[i * stride + attr]);
                output[i] = (T)newValue;
            }
            return output;
        }
    }
}
