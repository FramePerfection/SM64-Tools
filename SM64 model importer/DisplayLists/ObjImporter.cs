using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SM64RAM;

namespace SM64ModelImporter
{
    public class ObjReader
    {
        struct FaceVertex
        {
            public int Vertex, Texture, Normal;
        }

        public static void Read(string fileName, ConversionSettings conversionSettings, ref DisplayList dsp, out Dictionary<string, TextureInfo> allMaterials, out string[] messages)
        {
            List<string> messageList = new List<string>();
            string currentObject = "";

            Subset LastFrame = null;
            List<Subset> Frames = new List<Subset>();
            List<Vertex> Vertices = new List<Vertex>();
            List<int> Indices = new List<int>();
            List<FaceVertex> faceVertices = new List<FaceVertex>();

            List<Vertex> dspVertexBuffer = new List<Vertex>();
            List<int[]> dspIndexBuffers = new List<int[]>();
            List<int> dspTextureKeys = new List<int>();
            int[] dspIndices;
            int minVertexIndex = 0;

            List<Vector3> positions = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            allMaterials = new Dictionary<string, TextureInfo>();
            allMaterials["<Undefined>"] = new TextureInfo(null);

            string[] fileNameSplit = fileName.Split(new char[] { '/', '\\' });
            string fileRelativePath = fileName.Remove(fileName.Length - (fileNameSplit[fileNameSplit.Length - 1].Length), fileNameSplit[fileNameSplit.Length - 1].Length);

            if (!System.IO.File.Exists(fileName))
            {
                messages = new string[] { "File " + fileName + " was not found." };
                return;
            }
            StreamReader rd = new StreamReader(fileName);
            StreamReader materialReader = null;

            while (!rd.EndOfStream)
            {
                string line = rd.ReadLine();
                string[] split = line.Split(' ');
                if (split[0] == "mtllib")
                {
                    materialReader = new StreamReader(fileRelativePath + line.Remove(0, split[0].Length + 1));
                    string currentMtl = null;
                    while (!materialReader.EndOfStream)
                    {
                        string matLine = materialReader.ReadLine();
                        string[] matSplit = matLine.Split(' ');
                        if (matSplit[0] == "newmtl")
                            currentMtl = matLine.Remove(0, matSplit[0].Length);
                        if (matSplit[0] == "map_Kd")
                        {
                            TextureInfo newTex;
                            string fullPath = matLine.Remove(0, matSplit[0].Length).Trim();
                            if (fullPath[1] != ':') fullPath = fileRelativePath + fullPath;
                            allMaterials.Add(currentMtl, newTex = new TextureInfo(fullPath));
                        }
                    }
                }
                else if (split[0] == "o") //Object
                {
                    if (LastFrame == null)
                        LastFrame = new Subset();
                    currentObject = split[1];
                }
                else if (split[0] == "v") // Vertex
                    positions.Add(cvt.ParseVector3(split, 1));
                else if (split[0] == "vn")// Vertex Normal
                    normals.Add(cvt.ParseVector3(split, 1));
                else if (split[0] == "vt") // Vertex TexCoord
                    texCoords.Add(cvt.parseVector2(split, 1));
                else if (split[0] == "usemtl")
                {

                    LastFrame.VertexBuffer = Vertices.ToArray();
                    LastFrame.IndexBuffer = Indices.ToArray();

                    dspIndices = new int[LastFrame.IndexBuffer.Length];
                    for (int i = 0; i < LastFrame.IndexBuffer.Length; i += 3)
                    {
                        dspIndices[i] = LastFrame.IndexBuffer[i + 2] + minVertexIndex;
                        dspIndices[i + 1] = LastFrame.IndexBuffer[i + 1] + minVertexIndex;
                        dspIndices[i + 2] = LastFrame.IndexBuffer[i] + minVertexIndex;
                    }

                    if (Indices.Count > 0)
                    {
                        dspIndexBuffers.Add(dspIndices);
                        LastFrame.CreatePatches();
                        Frames.Add(LastFrame);
                    }
                    Indices.Clear();
                    LastFrame = new Subset();
                    string mtlName = line.Remove(0, split[0].Length);
                    if (!allMaterials.TryGetValue(mtlName, out LastFrame.Texture))
                    {
                        messageList.Add("Object " + currentObject + " has unassigned faces.");
                        LastFrame.Texture = allMaterials["<Undefined>"];
                    }
                }
                else if (split[0] == "f") // Face
                {
                    int i0 = 0, ix = 0;
                    for (int i = 1; i < split.Length; i++)
                    {
                        string[] vertexIndices = split[i].Split('/');
                        FaceVertex faceVertex = new FaceVertex();
                        int.TryParse(vertexIndices[0], out faceVertex.Vertex);
                        if (vertexIndices.Length > 1)
                            int.TryParse(vertexIndices[1], out faceVertex.Texture);
                        if (vertexIndices.Length > 2)
                            int.TryParse(vertexIndices[2], out faceVertex.Normal);
                        int index = 0;
                        foreach (FaceVertex v in faceVertices)
                        {
                            if (v.Vertex == faceVertex.Vertex && v.Texture == faceVertex.Texture && v.Normal == faceVertex.Normal) break;
                            index++;
                        }
                        if (i > 3)
                        {
                            Indices.Add(i0);
                            Indices.Add(ix);
                        }
                        Indices.Add(index);

                        if (i == 1) i0 = index;
                        ix = index;
                        if (index == faceVertices.Count)
                        {
                            faceVertices.Add(faceVertex);
                            Vertex newVertex;
                            Vector2 texCoord = Vector2.Empty;
                            if (faceVertex.Texture > 0) texCoord = texCoords[faceVertex.Texture - 1];
                            Vector3 normal = Vector3.Empty;
                            if (faceVertex.Normal > 0) normal = normals[faceVertex.Normal - 1];

                            Vertices.Add(newVertex = new Vertex(positions[faceVertex.Vertex - 1], texCoord, normal));
                            dspVertexBuffer.Add(newVertex);
                        }
                    }
                }
            }
            rd.BaseStream.Close();
            materialReader.Close();
            LastFrame.IndexBuffer = Indices.ToArray();
            LastFrame.VertexBuffer = Vertices.ToArray();

            dspIndices = new int[LastFrame.IndexBuffer.Length];
            for (int i = 0; i < LastFrame.IndexBuffer.Length; i += 3)
            {
                dspIndices[i] = LastFrame.IndexBuffer[i + 2] + minVertexIndex;
                dspIndices[i + 1] = LastFrame.IndexBuffer[i + 1] + minVertexIndex;
                dspIndices[i + 2] = LastFrame.IndexBuffer[i] + minVertexIndex;
            }
            dspIndexBuffers.Add(dspIndices);

            LastFrame.CreatePatches();
            Frames.Add(LastFrame);

            dsp.subsets = Frames.ToArray();
            messages = messageList.ToArray();
        }
    }
}
