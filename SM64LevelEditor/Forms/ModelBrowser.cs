using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SM64Renderer;
using SM64RAM;
using DX = Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Diagnostics;

namespace SM64LevelEditor
{
    public partial class ModelBrowser : Form
    {
        bool running = false;
        public DeviceWrapper device;
        Renderer renderer;
        GeoLayout current;
        int currentIndex = 0;
        bool needsRefresh = true;
        public ModelBrowser(DeviceWrapper device)
        {
            this.device = device;
            FormClosing += (_, __) => { __.Cancel = true; Hide(); };
            InitializeComponent();
            Shown += (_, __) =>
            {
                RunLoop();
                Render();
            };

            MouseWheel += (_, __) =>
            {
                if (Level.modelIDs.Count == 0) return;
                if (current != null) current.MakeInvisible(renderer);
                if (__.Delta > 0)
                    while (!Level.modelIDs.TryGetValue((byte)(currentIndex = (currentIndex + 1) % 256), out current)) ;
                else
                    while (!Level.modelIDs.TryGetValue((byte)(currentIndex = (currentIndex + 255) % 256), out current)) ;
                current.MakeVisible(renderer);
                lblModelID.Text = "Model ID: 0x" + currentIndex.ToString("X2") + " (" + currentIndex.ToString() + ")";
                needsRefresh = true;
            };
        }

        void RunLoop()
        {
            if (running)
                return;
            running = true;
            renderer = new Renderer(device);
        }

        public void Render()
        {
            if (!running || !needsRefresh)
                return;
            DX.Vector3 low, high;
            if (current != null && current.GetBounds(out low, out high))
            {
                DX.Vector3 center = (low + high) * 0.5f;
                DX.Vector3 size = high - low;
                float max = Math.Max(size.X, Math.Max(size.Y, size.Z));
                renderer.view = DX.Matrix.LookAtRH(center + new DX.Vector3(0, max * 1.5f, -max * 2), center, new DX.Vector3(0, 1, 0));
            }
            needsRefresh = false;
            renderer.clearColor = System.Drawing.Color.FromArgb(0, 15, 45);
            renderer.RenderFrame();
            device.device.Present(this);
        }
    }
}
