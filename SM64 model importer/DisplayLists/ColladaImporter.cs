using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;
using SM64RAM;
using System.Reflection;

namespace SM64ModelImporter
{
    public class ColladaImporter
    {

        //ignore warnings realted to these variables being unassigned. They will be set via reflection.
        struct Color { public float R, G, B, A;}
        struct TexCoord { public float S, T;}
        struct Vec3
        {
            public float X, Y, Z;
            public Vec3 TransformPosition(Matrix mat)
            {
                Vec3 result;
                result.X = mat.m[0 + 0] * X + mat.m[1 + 0] * Y + mat.m[2 + 0] * Z + mat.m[3 + 0];
                result.Y = mat.m[0 + 4] * X + mat.m[1 + 4] * Y + mat.m[2 + 4] * Z + mat.m[3 + 4];
                result.Z = mat.m[0 + 8] * X + mat.m[1 + 8] * Y + mat.m[2 + 8] * Z + mat.m[3 + 8];
                return result;
            }
            public Vec3 TransformNormal(Matrix mat)
            {
                Vec3 result;
                result.X = mat.m[0 + 0] * X + mat.m[1 + 0] * Y + mat.m[2 + 0] * Z;
                result.Y = mat.m[0 + 4] * X + mat.m[1 + 4] * Y + mat.m[2 + 4] * Z;
                result.Z = mat.m[0 + 8] * X + mat.m[1 + 8] * Y + mat.m[2 + 8] * Z;
                return result;
            }
        }
        struct Matrix
        {
            static Matrix()
            {
                Matrix _;
                _.m = new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
                identity = _;
            }
            public static Matrix identity { get; private set; }
            public float[] m;
            public Matrix InvertTranspose()
            {
                float[] inv = new float[16];
                inv[0] = m[5] * m[10] * m[15] -
                         m[5] * m[11] * m[14] -
                         m[9] * m[6] * m[15] +
                         m[9] * m[7] * m[14] +
                         m[13] * m[6] * m[11] -
                         m[13] * m[7] * m[10];

                inv[1] = -m[4] * m[10] * m[15] +
                          m[4] * m[11] * m[14] +
                          m[8] * m[6] * m[15] -
                          m[8] * m[7] * m[14] -
                          m[12] * m[6] * m[11] +
                          m[12] * m[7] * m[10];

                inv[2] = m[4] * m[9] * m[15] -
                         m[4] * m[11] * m[13] -
                         m[8] * m[5] * m[15] +
                         m[8] * m[7] * m[13] +
                         m[12] * m[5] * m[11] -
                         m[12] * m[7] * m[9];

                inv[3] = -m[4] * m[9] * m[14] +
                           m[4] * m[10] * m[13] +
                           m[8] * m[5] * m[14] -
                           m[8] * m[6] * m[13] -
                           m[12] * m[5] * m[10] +
                           m[12] * m[6] * m[9];

                inv[4] = -m[1] * m[10] * m[15] +
                          m[1] * m[11] * m[14] +
                          m[9] * m[2] * m[15] -
                          m[9] * m[3] * m[14] -
                          m[13] * m[2] * m[11] +
                          m[13] * m[3] * m[10];

                inv[5] = m[0] * m[10] * m[15] -
                         m[0] * m[11] * m[14] -
                         m[8] * m[2] * m[15] +
                         m[8] * m[3] * m[14] +
                         m[12] * m[2] * m[11] -
                         m[12] * m[3] * m[10];

                inv[6] = -m[0] * m[9] * m[15] +
                          m[0] * m[11] * m[13] +
                          m[8] * m[1] * m[15] -
                          m[8] * m[3] * m[13] -
                          m[12] * m[1] * m[11] +
                          m[12] * m[3] * m[9];

                inv[7] = m[0] * m[9] * m[14] -
                          m[0] * m[10] * m[13] -
                          m[8] * m[1] * m[14] +
                          m[8] * m[2] * m[13] +
                          m[12] * m[1] * m[10] -
                          m[12] * m[2] * m[9];

                inv[8] = m[1] * m[6] * m[15] -
                         m[1] * m[7] * m[14] -
                         m[5] * m[2] * m[15] +
                         m[5] * m[3] * m[14] +
                         m[13] * m[2] * m[7] -
                         m[13] * m[3] * m[6];

                inv[9] = -m[0] * m[6] * m[15] +
                          m[0] * m[7] * m[14] +
                          m[4] * m[2] * m[15] -
                          m[4] * m[3] * m[14] -
                          m[12] * m[2] * m[7] +
                          m[12] * m[3] * m[6];

                inv[10] = m[0] * m[5] * m[15] -
                          m[0] * m[7] * m[13] -
                          m[4] * m[1] * m[15] +
                          m[4] * m[3] * m[13] +
                          m[12] * m[1] * m[7] -
                          m[12] * m[3] * m[5];

                inv[11] = -m[0] * m[5] * m[14] +
                           m[0] * m[6] * m[13] +
                           m[4] * m[1] * m[14] -
                           m[4] * m[2] * m[13] -
                           m[12] * m[1] * m[6] +
                           m[12] * m[2] * m[5];

                inv[12] = -m[1] * m[6] * m[11] +
                          m[1] * m[7] * m[10] +
                          m[5] * m[2] * m[11] -
                          m[5] * m[3] * m[10] -
                          m[9] * m[2] * m[7] +
                          m[9] * m[3] * m[6];

                inv[13] = m[0] * m[6] * m[11] -
                         m[0] * m[7] * m[10] -
                         m[4] * m[2] * m[11] +
                         m[4] * m[3] * m[10] +
                         m[8] * m[2] * m[7] -
                         m[8] * m[3] * m[6];

                inv[14] = -m[0] * m[5] * m[11] +
                           m[0] * m[7] * m[9] +
                           m[4] * m[1] * m[11] -
                           m[4] * m[3] * m[9] -
                           m[8] * m[1] * m[7] +
                           m[8] * m[3] * m[5];

                inv[15] = m[0] * m[5] * m[10] -
                          m[0] * m[6] * m[9] -
                          m[4] * m[1] * m[10] +
                          m[4] * m[2] * m[9] +
                          m[8] * m[1] * m[6] -
                          m[8] * m[2] * m[5];
                Matrix result;
                result.m = inv;
                return result;
            }
        }

        public static void Read(string fileName, ConversionSettings conversionSettings, ref DisplayList dsp, out Dictionary<string, TextureInfo> allMaterials, out string[] messages)
        {
            messages = new string[0];
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
            List<Subset> meshes = new List<Subset>();
            XmlNode imageLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_images", mgr);
            XmlNode materialLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_materials", mgr);
            XmlNode effectLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_effects", mgr);
            XmlNode geometryLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_geometries", mgr);
            XmlNode sceneLibraryNode = doc.SelectSingleNode("//df:COLLADA/df:library_visual_scenes", mgr);
            XmlNodeList geometryNodes = sceneLibraryNode.SelectNodes("df:visual_scene/df:node/df:instance_geometry", mgr);
            bool flipYZ = doc.SelectSingleNode("//df:COLLADA/df:asset/df:up_axis", mgr).InnerText == "Z_UP";

            allMaterials = new Dictionary<string, TextureInfo>();
            allMaterials["<Undefined>"] = new TextureInfo(null);

            foreach (XmlNode geometryNode in geometryNodes)
            {
                XmlNode meshNode = geometryLibraryNode.SelectSingleNode("df:geometry[@id='" + geometryNode.Attributes["url"].Value.Remove(0, 1) + "']/df:mesh", mgr);
                XmlNode transformNode = geometryNode.SelectSingleNode("../df:matrix[@sid='transform']", mgr);
                Matrix transform = Matrix.identity;
                if (transformNode != null)
                    transform.m = Array.ConvertAll<string, float>(transformNode.InnerText.Split(' '), (s) => float.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
                Matrix normalTransform = transform.InvertTranspose();
                foreach (XmlNode trianglesNode in meshNode.SelectNodes("df:triangles", mgr))
                {
                    Subset meshSubset = new Subset();
                    meshes.Add(meshSubset);
                    List<Vertex> vertexBuffer = new List<Vertex>(); //final vertex buffer for this material
                    List<int> indexBuffer = new List<int>(); //final index buffer for this material

                    //Data to be read from XML
                    XmlAttribute materialAttribute = trianglesNode.Attributes["material"];
                    if (materialAttribute == null)
                    {
                        messages = new string[] { "Mesh without materials found. Skipping..." };
                        continue;
                    }
                    string material = materialAttribute.Value;
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
                        fullPath = Uri.UnescapeDataString(fullPath);
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
                        position = position.TransformPosition(transform);
                        Vec3 normal = normals == null ? new Vec3() : normals[indices[i * stride + normalOffset]];
                        normal = normal.TransformNormal(normalTransform);
                        TexCoord texCoord = texCoords == null ? new TexCoord() : texCoords[indices[i * stride + texCoordOffset]];
                        Color color = colors == null ? defaultColor : colors[indices[i * stride + colorOffset]];
                        vertices[i] = flipYZ ? new Vertex(new Vector3(position.X, position.Z, -position.Y), new Vector2(texCoord.S, texCoord.T), new Vector3(normal.X, normal.Z, -normal.Y))
                            : new Vertex(new Vector3(position.X, position.Y, position.Z), new Vector2(texCoord.S, texCoord.T), new Vector3(normal.X, normal.Y, normal.Z));
                        Color c = colors == null ? defaultColor : colors[indices[i * stride + 3]];
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
