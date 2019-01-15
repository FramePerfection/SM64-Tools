using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SM64Renderer;
using SM64RAM;
using Microsoft.DirectX;

namespace SM64LevelEditor
{
    public class GeoLayout
    {

        public class Node
        {
            List<Renderer.DisplayListEntry> dls = new List<Renderer.DisplayListEntry>();
            public Node parent { get; private set; }
            List<Node> children = new List<Node>();
            Matrix localTransform = Matrix.Identity;

            public Node AddChild()
            {
                Node n = new Node();
                n.parent = this;
                children.Add(n);
                return n;
            }

            void AddChild(Node node)
            {
                node.parent = this;
                children.Add(node);
            }

            public void AddDisplayList(DisplayList dl, int layer)
            {
                Renderer.DisplayListEntry entry = new Renderer.DisplayListEntry();
                entry.dl = dl;
                entry.layer = layer;
                dls.Add(entry);
            }

            public void MakeVisible(Renderer renderer)
            {
                foreach (Renderer.DisplayListEntry entry in dls)
                    if (!renderer.layers[entry.layer].Contains(entry))
                        renderer.layers[entry.layer].Add(entry);
                foreach (Node child in children)
                    child.MakeVisible(renderer);
            }
            public void MakeInvisible(Renderer renderer)
            {
                foreach (Renderer.DisplayListEntry entry in dls)
                    renderer.layers[entry.layer].Remove(entry);
                foreach (Node child in children)
                    child.MakeInvisible(renderer);
            }

            public Node DeepCopy()
            {
                Node newNode = new Node();
                foreach (Renderer.DisplayListEntry dl in dls)
                    newNode.AddDisplayList(dl.dl, dl.layer);
                foreach (Node n in children)
                    newNode.AddChild(n.DeepCopy());
                return newNode;
            }

            public void SetTransform(Matrix transform)
            {
                this.localTransform = transform;
                transform = GetGlobalTransform();
                foreach (Renderer.DisplayListEntry entry in dls)
                    entry.transform = transform;
                foreach (GeoLayout.Node child in children)
                    child.SetTransform(child.localTransform);
            }

            public void SetPickIndex(int index)
            {
                foreach (Renderer.DisplayListEntry entry in dls)
                    entry.pickIndex = index;
                foreach (Node child in children)
                    child.SetPickIndex(index);
            }

            public bool GetBounds(ref Microsoft.DirectX.Vector3 low, ref Microsoft.DirectX.Vector3 high)
            {
                bool result = false;
                foreach (Node n in children)
                    result |= n.GetBounds(ref low, ref high);
                foreach (var dl in dls)
                {
                    Microsoft.DirectX.Vector3[] vertices = dl.dl.GetVertexPositions();
                    foreach (Microsoft.DirectX.Vector3 vertex in vertices)
                    {
                        low.X = Math.Min(vertex.X, low.X);
                        low.Y = Math.Min(vertex.Y, low.Y);
                        low.Z = Math.Min(vertex.Z, low.Z);
                        high.X = Math.Max(vertex.X, high.X);
                        high.Y = Math.Max(vertex.Y, high.Y);
                        high.Z = Math.Max(vertex.Z, high.Z);
                    }
                    result = true;
                }
                return result;
            }

            public Matrix GetGlobalTransform() { return parent == null ? localTransform : localTransform * parent.GetGlobalTransform(); }

            public void Export(StreamWriter wr, string objectName, float scale, ref int vertexOffset)
            {
                int iDL = 0;
                foreach (SM64Renderer.Renderer.DisplayListEntry dl in dls)
                    dl.dl.Export(wr, objectName + iDL++, scale, ref vertexOffset);
                int iN = 0;
                foreach (Node n in children)
                    n.Export(wr, objectName + " c" + iN++, scale, ref vertexOffset);
            }
            public void ExportMaterials(StreamWriter wr, string pathRoot, string materialDirectory)
            {
                foreach (SM64Renderer.Renderer.DisplayListEntry dl in dls)
                    dl.dl.ExportMaterials(wr, pathRoot, materialDirectory);
                foreach (Node n in children)
                    n.ExportMaterials(wr, pathRoot, materialDirectory);
            }
        }

        public static TextureManager textureManager;

        static GeoLayout current;
        static Node currentNode;
        static bool validGeoLayout = false;

        static GeoLayout()
        {
            GeoLayoutReader.executors[(byte)GEO_LAYOUT_COMMANDS.START_HIERARCHY_NODE].Add(START_HIERARCHY_NODE);
            GeoLayoutReader.executors[(byte)GEO_LAYOUT_COMMANDS.END_HIERARCHY_NODE].Add(END_HIERARCHY_NODE);
            GeoLayoutReader.executors[(byte)GEO_LAYOUT_COMMANDS.DISPLAYLIST].Add(DISPLAYLIST);
            GeoLayoutReader.executors[(byte)GEO_LAYOUT_COMMANDS.TRANSFORMED_DISPLAYLIST].Add(TRANSFORMED_DISPLAYLIST);
            GeoLayoutReader.executors[(byte)GEO_LAYOUT_COMMANDS.GEO_LAYOUT_END].Add(VALIADATE_GEO_LAYOUT);
        }

        public static GeoLayout LoadSegmented(int segmentedPointer)
        {
            validGeoLayout = false;
            current = new GeoLayout();
            current.segmentedPointer = segmentedPointer;
            currentNode = current.root = new Node();
            GeoLayoutReader.ReadFromSegmented(segmentedPointer);
            if (validGeoLayout) return current;
            return null;
        }

        static bool VALIADATE_GEO_LAYOUT(byte[] commandBytes) { validGeoLayout = true; return false; }

        static bool START_HIERARCHY_NODE(byte[] commandBytes)
        {
            if (current == null) return false;
            currentNode = currentNode.AddChild();
            return true;
        }

        static bool END_HIERARCHY_NODE(byte[] commandBytes)
        {
            if (current == null || currentNode == null) return false;
            if (currentNode.parent != null)
                currentNode = currentNode.parent;
            return true;
        }

        static bool DISPLAYLIST(byte[] commandBytes)
        {
            if (current == null || currentNode == null) return false;
            if (textureManager == null)
                throw new Exception("Texture Manager must be set!");
            DisplayList newDL = DisplayListReader.Read(cvt.int32(commandBytes, 4), EmulationState.instance, textureManager);
            if (newDL != null)
            {
                newDL.Build(Level.renderer);
                currentNode.AddDisplayList(newDL, commandBytes[1]);
            }
            return true;
        }

        static bool TRANSFORMED_DISPLAYLIST(byte[] commandBytes)
        {
            if (current == null || currentNode == null) return false;
            if (textureManager == null)
                throw new Exception("Texture Manager must be set!");
            int ptr = cvt.int32(commandBytes, 8);
            DisplayList newDL = null;
            if (ptr != 0)
                newDL = DisplayListReader.Read(ptr, EmulationState.instance, textureManager);
            if (newDL != null)
            {
                newDL.Build(Level.renderer);
                currentNode.AddDisplayList(newDL, commandBytes[1]);
            }
            return true;
        }

        public int segmentedPointer { get; private set; }
        Node root;

        public void Export(string filename)
        {
            string objectName = Path.GetFileNameWithoutExtension(filename);
            string path = Path.GetDirectoryName(filename);
            string materialDirectory = path + "/" + objectName;
            StreamWriter wr = new StreamWriter(filename);
            wr.WriteLine("mtllib " + objectName + ".mtl");
            if (!Directory.Exists(materialDirectory))
                Directory.CreateDirectory(materialDirectory);
            int vertexOffset = 1;
            root.Export(wr, objectName, 0.01f, ref vertexOffset);
            wr.Close();
            wr = new StreamWriter(materialDirectory + ".mtl");
            root.ExportMaterials(wr, path, materialDirectory);
            wr.Close();
        }

        public bool GetBounds(out Microsoft.DirectX.Vector3 low, out Microsoft.DirectX.Vector3 high)
        {
            low = high = new Microsoft.DirectX.Vector3();
            return root.GetBounds(ref low, ref high);
        }

        public void MakeVisible(Renderer renderer)
        {
            root.MakeVisible(renderer);
        }

        public void MakeInvisible(Renderer renderer)
        {
            root.MakeInvisible(renderer);
        }

        public void SetTransform(Matrix transform) { root.SetTransform(transform); }

        public void SetPickIndex(int index) { root.SetPickIndex(index); }

        public GeoLayout DeepCopy()
        {
            GeoLayout output = new GeoLayout();
            output.root = root.DeepCopy();
            return output;
        }
    }
}
