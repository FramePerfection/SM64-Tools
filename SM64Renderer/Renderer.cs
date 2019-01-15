using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;
using System.Reflection;

namespace SM64Renderer
{
    public class Renderer
    {
        public static class EffectHandles
        {
            public static EffectHandle World, ViewProjection, ShiftMul, Texture0, Texture1, Color, PickColor;
            static EffectHandles()
            {
                foreach (FieldInfo field in typeof(EffectHandles).GetFields())
                    if (field.IsStatic && field.FieldType == typeof(EffectHandle))
                        field.SetValue(null, EffectHandle.FromString(field.Name));
            }
        }

        public class DisplayListEntry
        {
            public int layer;
            public DisplayList dl;
            public Matrix transform = Matrix.Identity;
            public int pickIndex;
        }
        public const int NUM_LAYERS = 8;

        static Vector3[] bboxVerts = new Vector3[] {new Vector3(-1, -1, -1), new Vector3(1, -1, -1), new Vector3(-1, 1, -1), new Vector3(1, 1, -1),
                                                    new Vector3(-1, -1, 1), new Vector3(1, -1, 1), new Vector3(-1, 1, 1), new Vector3(1, 1, 1)};
        static short[] bboxLineInds = new short[] { 0, 1, 2, 3, 0, 2, 1, 3, 4, 5, 6, 7, 4, 6, 5, 7, 0, 4, 1, 5, 2, 6, 3, 7 };
        static short[] bboxFillInds = new short[] { 0, 1, 2, 1, 3, 2, 6, 5, 4, 6, 7, 5,
                                                    4, 0, 6, 0, 2, 6, 7, 1, 5, 7, 3, 1,
                                                    4, 1, 0, 4, 5, 1, 2, 3, 6, 3, 7, 6};
        static CustomVertex.PositionColored[] axisVerts = new CustomVertex.PositionColored[] {
            new CustomVertex.PositionColored(Vector3.Empty, unchecked((int)0xFF0000FF)), new CustomVertex.PositionColored(new Vector3(-1, 0, 0), unchecked((int)0xFF0000FF)),
            new CustomVertex.PositionColored(Vector3.Empty, unchecked((int)0xFF00FF00)), new CustomVertex.PositionColored(new Vector3(0, 1, 0), unchecked((int)0xFF00FF00)),
            new CustomVertex.PositionColored(Vector3.Empty, unchecked((int)0xFFFF0000)), new CustomVertex.PositionColored(new Vector3(0, 0, 1), unchecked((int)0xFFFF0000))};

        public static Renderer current = null;

        public Color clearColor = Color.SkyBlue;
        public List<DisplayListEntry>[] layers = new List<DisplayListEntry>[NUM_LAYERS];
        public bool[] hideLayers = new bool[NUM_LAYERS];
        public DeviceWrapper device;

        public Effect defaultEffect, bboxEffect;
        public VertexDeclaration vtxDeclaration;
        public VertexDeclaration vec3Declaration, axisDeclaration;

        Matrix viewProjection;
        public Matrix view = Matrix.LookAtLH(new Vector3(), new Vector3(0, 0, 1), new Vector3(0, 1, 0)), projection = Matrix.PerspectiveFovRH(1, 1, 5f, 50000);

        public static Vector4 pickIndexToColor(int index)
        {
            return new Vector4((index % 256) / 255f, ((index >> 8) % 256) / 255f, ((index >> 16) % 256) / 255f, 1);
        }

        public Renderer(DeviceWrapper device)
        {
            this.device = device;
            for (int i = 0; i < layers.Length; i++)
                layers[i] = new List<DisplayListEntry>();

            defaultEffect = device.LoadEffect(Properties.Resources.DefaultEffect);
            bboxEffect = device.LoadEffect(Properties.Resources.BoundingBox);
            vtxDeclaration = new VertexDeclaration(device, new VertexElement[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(0, 20, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1),
                VertexElement.VertexDeclarationEnd});
            vec3Declaration = new VertexDeclaration(device, new VertexElement[] { new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0), VertexElement.VertexDeclarationEnd });
            axisDeclaration = new VertexDeclaration(device, new VertexElement[] { 
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0), 
                new VertexElement(0, 12, DeclarationType.Ubyte4, DeclarationMethod.Default, DeclarationUsage.Color, 0), 
                VertexElement.VertexDeclarationEnd });
        }

        public void RenderFrame()
        {
            current = this;
            viewProjection = view * projection;
            device.device.Clear(ClearFlags.Target | ClearFlags.ZBuffer | ClearFlags.Stencil, clearColor.ToArgb(), 1, 0);
            device.device.BeginScene();
            for (int i = 0; i < layers.Length; i++)
            {
                if (hideLayers[i]) continue;
                foreach (DisplayListEntry dl in layers[i])
                    dl.dl.Draw(dl.transform, dl.pickIndex);
            }
            device.device.EndScene();
            current = null;
        }

        public void SetValues(Effect e, Matrix transform)
        {
            e.SetValue(EffectHandles.ViewProjection, viewProjection);
            e.SetValue(EffectHandles.World, transform);
        }

        public void ClearLayers()
        {
            for (int i = 0; i < layers.Length; i++)
                layers[i].Clear();
        }

        public void DrawBoudingBox(Matrix transform, Vector4 color)
        {
            SetValues(bboxEffect, transform);
            bboxEffect.SetValue(EffectHandles.Color, color);
            bboxEffect.Begin(FX.None);
            bboxEffect.BeginPass(0);
            device.device.VertexDeclaration = vec3Declaration;
            device.device.DrawIndexedUserPrimitives(PrimitiveType.LineList, 0, 8, 12, bboxLineInds, true, bboxVerts);
            bboxEffect.EndPass();
            bboxEffect.End();
        }

        public void DrawAxis(Matrix transform)
        {
            SetValues(bboxEffect, transform);
            bboxEffect.Begin(FX.None);
            bboxEffect.BeginPass(2);
            device.device.VertexDeclaration = axisDeclaration;
            device.device.DrawUserPrimitives(PrimitiveType.LineList, 3, axisVerts);
            bboxEffect.EndPass();
            bboxEffect.End();
        }

        public void FillPickingBox(Matrix transform, int pickIndex)
        {
            SetValues(bboxEffect, transform);
            bboxEffect.SetValue(EffectHandles.Color, Renderer.pickIndexToColor(pickIndex));
            bboxEffect.Begin(FX.None);
            bboxEffect.BeginPass(1);
            device.device.VertexDeclaration = vec3Declaration;
            device.device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0, 8, 12, bboxFillInds, true, bboxVerts);
            bboxEffect.EndPass();
            bboxEffect.End();
        }
    }
}
