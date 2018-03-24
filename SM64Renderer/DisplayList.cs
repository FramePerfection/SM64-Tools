using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using DX = Microsoft.DirectX;
using D3D = Microsoft.DirectX.Direct3D;
using SM64RAM;

namespace SM64Renderer
{
    public class DisplayList
    {
        public struct Vertex
        {
            public DX.Vector3 position;
            public DX.Vector2 texCoord;
            public DX.Vector4 normal;

            public Vertex(DX.Vector3 position, DX.Vector2 texCoord, DX.Vector3 n, float a)
            {
                this.position = position;
                this.texCoord = texCoord;
                n.Normalize();
                this.normal = new DX.Vector4(n.X, n.Y, n.Z, a);
            }
            public Vertex(DX.Vector3 position, DX.Vector2 texCoord, DX.Vector4 c)
            {
                this.position = position;
                this.texCoord = texCoord;
                this.normal = c;
            }
        }

        class Subset
        {
            public TextureInfo texture;
            public D3D.Texture texture0, texture1;
            public D3D.IndexBuffer indexBuffer;
            public int[] indices;
            public int tile;
            public int maskT, maskS;
            public int shiftT, shiftS;
            public int cmS, cmT;
            public int line;
        }

        D3D.VertexBuffer vertexBuffer;
        D3D.Effect usedEffect;
        Renderer renderer;
        TextureManager texMan;
        List<Subset> subsets = new List<Subset>();
        Vertex[] vertices;
        Dictionary<int, Vertex> vertexAddresses = new Dictionary<int, Vertex>();
        Dictionary<int, int> vertexIndices = new Dictionary<int, int>();
        List<int> tmpIndexList = new List<int>();
        int tmpSegmentedPointer;
        int[] tmpVertexAddresses = new int[16], tmpVertexIndices = new int[16];
        int tmpMaxVertex = 0;
        int tmpLastTile = 0;
        Subset tmpSubset = new Subset();

        public DisplayList(TextureManager texMan)
        {
            this.texMan = texMan;
        }

        public void Build(Renderer renderer)
        {
            FinalizeSubset();
            this.renderer = renderer;
            vertices = new Vertex[vertexAddresses.Count];
            int i = 0;
            foreach (KeyValuePair<int, Vertex> a in vertexAddresses)
                vertices[i++] = a.Value;
            foreach (Subset sub in subsets)
            {
                sub.indexBuffer = new D3D.IndexBuffer(typeof(int), sub.indices.Length, renderer.device, D3D.Usage.None, D3D.Pool.Managed);
                sub.indexBuffer.SetData(sub.indices, 0, D3D.LockFlags.Discard);
            }
            if (tmpMaxVertex == 0) tmpMaxVertex = 1;
            vertexBuffer = new D3D.VertexBuffer(typeof(Vertex), tmpMaxVertex, renderer.device, D3D.Usage.None, D3D.VertexFormats.Position, D3D.Pool.Managed);
            vertexBuffer.SetData(vertices, 0, D3D.LockFlags.Discard);

            usedEffect = renderer.defaultEffect;
        }

        void FinalizeSubset()
        {
            tmpSubset.indices = tmpIndexList.ToArray();
            if (tmpSubset.indices.Length > 0)
            {
                tmpSubset.texture0 = texMan.LoadByInfo(tmpSubset.texture);
                subsets.Add(tmpSubset);
                tmpSubset = new Subset();
            }
            tmpIndexList.Clear();
        }

        public void LoadVertices(long command)
        {
            int w0 = (int)(command >> 0x20);
            int numVertices = Utility.GetBits(w0, 12, 4);
            //int numVertices2 = Utility.GetBits(w0, 4, 16);
            tmpSegmentedPointer = (int)(command & 0xFFFFFFFF);
            if (!EmulationState.instance.AssertRead(tmpSegmentedPointer, numVertices * 0x10)) return;
            EmulationState.RAMBank bank = EmulationState.instance.banks[tmpSegmentedPointer >> 0x18];
            for (int i = 0; i < numVertices; i++)
            {
                int offset = tmpSegmentedPointer + i * 0x10;
                if (vertexIndices.TryGetValue(offset, out tmpVertexIndices[i]))
                    continue;
                tmpVertexAddresses[i] = offset;
                tmpVertexIndices[i] = tmpMaxVertex++;
                int o = offset & 0xFFFFFF;
                vertexAddresses[offset] = new Vertex(new DX.Vector3(cvt.int16(bank.value, o + 0), cvt.int16(bank.value, o + 2), cvt.int16(bank.value, o + 4)),
                                                    new DX.Vector2(Utility.Fixed2Float(cvt.int16(bank.value, o + 8), 5), Utility.Fixed2Float(cvt.int16(bank.value, o + 10), 5)),
                                                    new DX.Vector4((sbyte)bank.value[o + 12] / (float)sbyte.MaxValue, (sbyte)bank.value[o + 13] / (float)sbyte.MaxValue, (sbyte)bank.value[o + 14] / (float)sbyte.MaxValue, bank.value[o + 15] / 255f));
            }
        }

        public void CreateTriangle(long command)
        {
            int w1 = (int)(command & 0xFFFFFFFF);
            int[] indexValues = new int[] { Utility.GetBits(w1, 8, 16) / 10, Utility.GetBits(w1, 8, 8) / 10, Utility.GetBits(w1, 8, 0) / 10 };
            for (int i = 0; i < 3; i++)
                tmpIndexList.Add(tmpVertexIndices[indexValues[i]]);
        }

        public void SetTile(long command)
        {
            FinalizeSubset();
            int w0 = (int)(command >> 0x20), w1 = (int)(command & 0xFFFFFFFF);
            tmpSubset.cmT = Utility.GetBits(w1, 2, 0x12);
            tmpSubset.maskT = Utility.GetBits(w1, 4, 0xE);
            tmpSubset.shiftT = Utility.GetBits(w1, 4, 0xA);
            tmpSubset.cmS = Utility.GetBits(w1, 2, 0x8);
            tmpSubset.maskS = Utility.GetBits(w1, 4, 0x4);
            tmpSubset.shiftS = Utility.GetBits(w1, 4, 0);
        }

        public void SetTileSize(long command)
        {
            FinalizeSubset();
            int w0 = (int)(command >> 0x20);
            int w1 = (int)(command & 0xFFFFFFFF);
            TextureManager.Tile tile = texMan.tiles[tmpLastTile = Utility.GetBits(w1, 4, 0x18)];
            tile.uls = Utility.GetBits(w0, 0xC, 0xC);
            tile.ult = Utility.GetBits(w0, 0xC, 0);
            tile.lrs = Utility.GetBits(w1, 0xC, 0xC);
            tile.lrt = Utility.GetBits(w1, 0xC, 0);

            tmpSubset.texture.width = (tile.lrs - tile.uls) / 4 + 1;
            tmpSubset.texture.height = (tile.lrt - tile.ult) / 4 + 1;
        }

        public void LoadBlock(long command)
        {
            FinalizeSubset();
            int w0 = (int)(command >> 0x20), w1 = (int)(command & 0xFFFFFFFF);
            TextureManager.Tile tile = texMan.tiles[tmpLastTile = Utility.GetBits(w1, 4, 0x18)];
            tile.uls = Utility.GetBits(w0, 0xC, 0xC);
            tile.ult = Utility.GetBits(w0, 0xC, 0);
            tmpSubset.texture.texels = Utility.GetBits(w1, 0xC, 0xC);
            tmpSubset.texture.dxt = Utility.GetBits(w1, 0xC, 0);

            tmpSubset.texture.height = tmpSubset.texture.width == 0 ? 0 : (tmpSubset.texture.texels + 1) / tmpSubset.texture.width;
        }

        public void SetTexture(long command)
        {
            FinalizeSubset();
            int w0 = (int)(command >> 0x20);
            int w1 = (int)(command & 0xFFFFFFFF);
            TextureManager.Tile tile = texMan.tiles[tmpLastTile = Utility.GetBits(w1, 3, 0x18)];
            tmpSubset.texture.segmentedPointer = (int)(command & 0xFFFFFFFF);
            tmpSubset.texture.fmt = Utility.GetBits(w0, 3, 0x15);
            tmpSubset.texture.bpp = Utility.GetBits(w0, 2, 0x13);
            int a = Utility.GetBits(w0, 0xC, 0);
            if (a != 0)
                tmpSubset.texture.width = a + 1;

            ushort tsX = (ushort)Utility.GetBits(w1, 16, 16);
            ushort tsY = (ushort)Utility.GetBits(w1, 16, 0);

            //tmpSubset.texture.width = (ushort)(tsX >> 6) + 1;
            //tmpSubset.texture.height = (ushort)(tsY >> 6) + 1;
            //tmpSubset.texture.height = tmpSubset.texture.width == 0 ? 0 : (tmpSubset.texture.texels + 1) / tmpSubset.texture.width;
        }

        public void Draw(DX.Matrix transform, int pickValue = 0)
        {
            D3D.Device device = renderer.device;
            device.VertexDeclaration = renderer.vtxDeclaration;
            device.SetStreamSource(0, vertexBuffer, 0);
            renderer.SetValues(usedEffect, transform);
            usedEffect.SetValue(Renderer.EffectHandles.PickColor, Renderer.pickIndexToColor(pickValue));

            foreach (Subset sub in subsets)
            {
                float shiftTValue = sub.shiftT >= 11 ? (int)1 << (0x10 - sub.shiftT) : 1.0f / (1 << sub.shiftT);
                float shiftSValue = sub.shiftS >= 11 ? (int)1 << (0x10 - sub.shiftS) : 1.0f / (1 << sub.shiftS);
                usedEffect.SetValue(Renderer.EffectHandles.ShiftMul, new DX.Vector4(shiftTValue / (float)sub.texture.width, shiftSValue / (float)sub.texture.height, 0, 0));
                usedEffect.SetValue(Renderer.EffectHandles.Texture0, sub.texture0);
                usedEffect.SetValue(Renderer.EffectHandles.Texture1, sub.texture1);
                usedEffect.Begin(D3D.FX.None);
                usedEffect.BeginPass(0);
                device.Indices = sub.indexBuffer;
                device.DrawIndexedPrimitives(D3D.PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, sub.indices.Length / 3);
                usedEffect.EndPass();
                usedEffect.End();
            }
        }

        public void Export(System.IO.StreamWriter targetStream, string objectName, float scale, ref int vertexOffset)
        {
            for (int i = 0; i < vertices.Length; i++)
                targetStream.WriteLine("v " + (vertices[i].position.X * scale).ToString(CultureInfo.InvariantCulture) + " " + (vertices[i].position.Y * scale).ToString(CultureInfo.InvariantCulture) + " " + (vertices[i].position.Z * scale).ToString(CultureInfo.InvariantCulture));
            for (int i = 0; i < vertices.Length; i++)
                targetStream.WriteLine("vt " + vertices[i].texCoord.X.ToString(CultureInfo.InvariantCulture) + " " + vertices[i].texCoord.Y.ToString(CultureInfo.InvariantCulture));
            for (int i = 0; i < vertices.Length; i++)
                targetStream.WriteLine("vn " + vertices[i].normal.X.ToString(CultureInfo.InvariantCulture) + " " + vertices[i].normal.Y.ToString(CultureInfo.InvariantCulture) + " " + vertices[i].normal.Z.ToString(CultureInfo.InvariantCulture));

            int iSubset = 0;
            foreach (Subset subset in subsets)
            {
                targetStream.WriteLine("o " + objectName + " " + iSubset++);
                targetStream.WriteLine("usemtl mtl" + subset.texture.segmentedPointer.ToString("X8"));
                for (int i = 0; i < subset.indices.Length; i += 3)
                    targetStream.WriteLine("f " + vertexIndex(subset.indices[i] + vertexOffset) + " " + vertexIndex(subset.indices[i + 1] + vertexOffset) + " " + vertexIndex(subset.indices[i + 2] + vertexOffset));
            }
            vertexOffset += (int)vertices.Length;
        }
        static string vertexIndex(int value) { return value + "/" + value + "/" + value; }
        public void ExportMaterials(System.IO.StreamWriter targetStream, string pathRoot, string materialDirectory)
        {
            string relativeMaterialPath = materialDirectory.Remove(0, pathRoot.Length + 1) + "/";
            foreach (Subset s in subsets)
            {
                string pngFile = materialDirectory + "/" + s.texture.segmentedPointer.ToString("X8") + ".png";
                if (System.IO.File.Exists(pngFile)) continue;
                if (s.texture0 != null)
                D3D.TextureLoader.Save(pngFile, D3D.ImageFileFormat.Png, s.texture0);
                targetStream.WriteLine("newmtl mtl" + s.texture.segmentedPointer.ToString("X8"));
                targetStream.WriteLine("map_Kd " + relativeMaterialPath + s.texture.segmentedPointer.ToString("X8") + ".png");
            }
        }
    }
}
