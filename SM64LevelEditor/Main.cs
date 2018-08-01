using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SM64Renderer;
using SM64RAM;
using System.Diagnostics;
using DX = Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace SM64LevelEditor
{
    public partial class Main : Form
    {
        ToolBox toolBox;
        LogWindow logWindow;

        public DeviceWrapper device;
        TextureManager globalTextures;
        Texture pickTexture;
        Surface pickSurface, pickSwapSurface;

        const int MAX_KEY = 255;
        public static bool[] keyDown = new bool[MAX_KEY], prevKeyDown = new bool[MAX_KEY];
        public static bool keyPress(int i) { return keyDown[i] && !prevKeyDown[i]; }
        public static bool keyRelease(int i) { return !keyDown[i] && prevKeyDown[i]; }
        public static DX.Vector2 mouseDelta = new DX.Vector2();
        public static float mouseDeltaZ = 0;
        public static Main instance;

        bool selectObject = false;

        public Main()
        {
            //Makes form closing faster
            FormClosing += (_, __) => System.Diagnostics.Process.GetCurrentProcess().Kill();

            instance = this;
            InitializeComponent();
            Level.InitCommands();

            Shown += RunLoop;
            KeyDown += (object sender, KeyEventArgs e) => keyDown[(int)e.KeyCode] = true;
            KeyUp += (object sender, KeyEventArgs e) => keyDown[(int)e.KeyCode] = false;
            int mX = 0, mY = 0;
            MouseMove += (object sender, MouseEventArgs e) =>
            {
                mouseDelta = new DX.Vector2(mX - e.X, mY - e.Y);
                if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                    Editor.camera.Rotate(mouseDelta, 0.01f);
                mX = e.X; mY = e.Y;
            };
            MouseDown += (object sender, MouseEventArgs e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    selectObject = true;
            };
            MouseWheel += (object sender, MouseEventArgs e) => { mouseDeltaZ = e.Delta; };
        }

        void CreatePickSurface(object sender, CancelEventArgs acken)
        {
            if (pickTexture != null)
            {
                pickTexture.Dispose();
                pickSurface.Dispose();
                pickSwapSurface.Dispose();
            }
            int w = ClientRectangle.Width, h = ClientRectangle.Height;
            pickTexture = new Texture(device.device, w, h, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
            pickSurface = pickTexture.GetSurfaceLevel(0);
            pickSwapSurface = device.device.CreateOffscreenPlainSurface(w, h, Format.A8R8G8B8, Pool.SystemMemory);
            device.device.SetRenderTarget(1, pickSurface);
        }

        void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SM64 ROMs (.z64)|*.z64";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            Editor.projectSettings = new ProjectSettings(ofd.FileName);

            EmulationState.instance.LoadROM(ofd.FileName);
            EmulationState.instance.LoadBank(0x2, 0x108A40, 0x114750, true);
            EmulationState.instance.LoadBank(0x15, 0x2ABCA0, 0x2AC6B0, false);
            EmulationState.instance.banks[0x15].ptrsROM.AddRange(new int[] { 0x2A6228, 0x26A114, 0x26A138 });

            globalTextures = new TextureManager(EmulationState.instance, Level.renderDevice);
            GeoLayout.textureManager = globalTextures;
            LevelScriptReader.ReadFrom(0x2ABCA0, 0x2ABE58);
            int[] levelAddresses = LevelScriptReader.GetCommands(0x2AC094, 0x2AC2FC, 0x00);
            toolBox.SetLevelAddresses(levelAddresses);
            if (levelAddresses.Length > 0)
                LoadLevel(levelAddresses[0]);
        }

        private void RunLoop(object sender, EventArgs e)
        {
            device = new DeviceWrapper();
            if (!device.Init(this))
            {
                MessageBox.Show("Unable to create Direct3D device!");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            device.device.DeviceResizing += CreatePickSurface;
            CreatePickSurface(null, null);
            Level.renderDevice = device;
            Level.renderer = new Renderer(Level.renderDevice);

            toolBox = new ToolBox(this);
            toolBox.Show();

            logWindow = new LogWindow();
            EmulationState.messages = logWindow;

            openToolStripMenuItem_Click(null, null);

            Stopwatch tm = new Stopwatch();
            float frameTime = 0;
            while (Created)
            {
                tm.Reset();
                tm.Start();
                UpdateFrame(frameTime);
                DrawFrame();
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
                tm.Stop();
                frameTime = (float)tm.ElapsedTicks / Stopwatch.Frequency;
            }
        }

        public void LoadLevel(int address)
        {
            try
            {
                Editor.currentLevel = Level.LoadLevelROM(address);
                Editor.currentAreaIndex = 0;
                Editor.currentLevel.MakeVisible(0);
                Editor.currentLevel.SetAliasFile(Editor.projectSettings.GetBehaviourAliasList(address), Editor.projectSettings.GetModelIDAliasList(address));
                areaToolStripMenuItem.DropDownItems.Clear();
                for (int i = 0; i < Editor.currentLevel.areas.Length; i++)
                    if (Editor.currentLevel.areas[i] != null)
                    {
                        int k = i;
                        var btna = new ToolStripButton("Area " + i.ToString());
                        btna.Click += (_, __) => Editor.currentAreaIndex = k;
                        areaToolStripMenuItem.DropDownItems.Add(btna);
                    }
            }
            catch (Exception ex)
            {
                EmulationState.messages.AppendMessage("Unable to load level! Exception was:\n" + ex.ToString(), "Error");
                Editor.currentLevel = null;
            }
        }

        void UpdateFrame(float time)
        {
            Editor.Update(time);
            for (int i = 0; i < keyDown.Length; i++)
                prevKeyDown[i] = keyDown[i];
            mouseDelta = DX.Vector2.Empty;
            mouseDeltaZ = 0;
        }

        void DrawFrame()
        {
            device.device.Present();
            if (Editor.currentLevel != null)
                Level.renderer.RenderFrame();
            device.device.BeginScene();
            if (Editor.currentArea != null)
                foreach (Object obj in Editor.currentArea)
                    Level.renderer.DrawBoudingBox(DX.Matrix.Scaling(100, 100, 100) * obj.GetTransform(), obj.GetBoundingBoxColor());

            device.device.RenderState.ZBufferWriteEnable = false;
            Level.renderer.DrawAxis(DX.Matrix.Scaling(100, 100, 100) * DX.Matrix.Translation(Editor.camera.cursor));
            Level.renderer.DrawBoudingBox(DX.Matrix.Scaling(100, 100, 100) * DX.Matrix.Translation(Editor.camera.cursor), new DX.Vector4(0, 1, 0, 1));
            device.device.RenderState.ZBufferWriteEnable = true;

            if (Editor.currentArea != null)
                foreach (Object obj in Editor.currentArea)
                    if (obj.geometry == null)
                        Level.renderer.FillPickingBox(DX.Matrix.Scaling(100, 100, 100) * obj.GetTransform(), obj.pickIndex);

            device.device.EndScene();
            if (selectObject && Editor.allowSelect)
            {
                device.device.GetRenderTargetData(pickSurface, pickSwapSurface);
                Point p = PointToClient(Cursor.Position);
                byte[] pixel = (byte[])pickSwapSurface.LockRectangle(typeof(byte), new Rectangle(p.X, p.Y, 1, 1), LockFlags.ReadOnly, new int[] { 4 });
                int pickedValue = pixel[2] | (pixel[1] << 8) | (pixel[0] << 16);
                pickSwapSurface.UnlockRectangle();
                selectObject = false;
                if (!keyDown[(int)Keys.ShiftKey])
                    Editor.currentArea.ClearSelection();
                Editor.currentArea.Select(pickedValue);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.currentLevel.CreateCommands(Editor.currentLevel.segmentedPointer);
            EmulationState.instance.SaveBank(Editor.currentLevel.segmentedPointer >> 0x18);
            System.IO.File.WriteAllBytes(EmulationState.instance.ROMName, EmulationState.instance.ROM);
        }

        private void currentAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.obj|*.obj";
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            Editor.currentArea.geometry.Export(sfd.FileName);
        }

        private void selectedModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            foreach (Object obj in Editor.currentArea.selectedObjects)
                if (obj.geometry != null)
                    obj.geometry.Export(dlg.SelectedPath + "\\" + obj.model_ID + ".obj");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmulationState.messages.AppendMessage("Not implemented", "Error");
        }

        private void toolBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolBoxToolStripMenuItem.Checked = !toolBoxToolStripMenuItem.Checked;
            if (toolBoxToolStripMenuItem.Checked)
                toolBox.Show();
            else
                toolBox.Hide();
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logToolStripMenuItem.Checked = !logToolStripMenuItem.Checked;
            if (logToolStripMenuItem.Checked)
                logWindow.Show();
            else
                logWindow.Hide();
        }

        private void RAMBankToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            RAMBankToolStripMenuItem.DropDownItems.Clear();
            RAMBankToolStripMenuItem.DropDownItems.Add(allToolStripMenuItem);
            RAMBankToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            for (int i = 0; i < EmulationState.instance.banks.Length; i++)
                if (EmulationState.instance.banks[i] != null)
                {
                    int local = i;
                    RAMBankToolStripMenuItem.DropDownItems.Add("Bank 0x" + i.ToString("X"), null, (_, __) => ExportBank(EmulationState.instance.banks[local]));
                }
        }

        void ExportBank(EmulationState.RAMBank bank)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            System.IO.File.WriteAllBytes(dlg.FileName, bank.value);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            for (int i = 0; i < EmulationState.instance.banks.Length; i++)
                if (EmulationState.instance.banks[i] != null)
                    System.IO.File.WriteAllBytes(dlg.SelectedPath + "\\Bank 0x" + i.ToString("X") + ".bin", EmulationState.instance.banks[i].value);
        }

        private void backgroundToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EmulationState.RAMBank backgroundBank = EmulationState.instance.banks[0xA]; //0xA holds background images.
            if (backgroundBank == null)
            {
                EmulationState.messages.AppendMessage("No background bank loaded for this level.", "Information");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "image files|*.png";
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            Bitmap final = new Bitmap(249, 249);
            Graphics g = Graphics.FromImage(final);
            g.Clear(Color.Black);
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                {
                    Bitmap patch = SM64Renderer.ImageConverter.ReadRGBA16(backgroundBank.value, (x + y * 8) * 0x800, 32, 32);
                    g.DrawImage(patch, x * 31, y * 31);
                }
            g.Flush();
            final.Save(sfd.FileName);
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundDialog dlg = new BackgroundDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (BankDescription bank in Editor.currentLevel.loadedBanks)
                    if (bank.type == BankDescription.BankType.Background)
                    {
                        bank.ROM_Start = dlg.selectedBank.ROM_Start;
                        bank.compressed = dlg.selectedBank.compressed;
                        bank.arg = dlg.selectedBank.arg;
                        if (dlg.imageFileName != null)
                        {
                            Bitmap bmp = (Bitmap)Bitmap.FromFile(dlg.imageFileName);
                            if (bmp.Width != 249 || bmp.Height != 249)
                            {
                                Bitmap destImage = new Bitmap(249, 249, PixelFormat.Format32bppArgb);

                                destImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

                                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(destImage))
                                {
                                    graphics.CompositingMode = CompositingMode.SourceCopy;
                                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    using (ImageAttributes wrapMode = new ImageAttributes())
                                    {
                                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                                        graphics.DrawImage(bmp, new Rectangle(0, 0, 249, 249), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, wrapMode);
                                    }
                                }
                                EmulationState.messages.AppendMessage("Selected image has been resized from " + bmp.Width + " x " + bmp.Height + " pixels to 256x256 pixels.", "Information");
                                bmp.Dispose();
                                bmp = destImage;
                            }

                            BitmapData dat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            byte[] bytes = new byte[bmp.Width * bmp.Height * 4];
                            System.Runtime.InteropServices.Marshal.Copy(dat.Scan0, bytes, 0, bytes.Length);
                            int stride = dat.Stride;
                            bmp.UnlockBits(dat);
                            byte[] newBankValue = new byte[256 * 256 * 2 + 10 * 8 * 4];
                            int cursor = 0;
                            for (int tileY = 0; tileY < 8; tileY++)
                                for (int tileX = 0; tileX < 8; tileX++)
                                    for (int y = 0; y < 32; y++)
                                        for (int x = 0; x < 32; x++)
                                        {
                                            int offset = ((x + tileX * 31) * 4 + (y + tileY * 31) * stride);
                                            short newValue = (short)(((bytes[offset + 2] & 0xF8) << 8) | ((bytes[offset + 1] & 0xF8) << 3) | (bytes[offset] >> 2));
                                            newBankValue[cursor++] = (byte)(newValue >> 8);
                                            newBankValue[cursor++] = (byte)(newValue & 0xFF);
                                        }
                            for (int y = 0; y < 8; y++)
                                for (int x = 0; x < 10; x++)
                                {
                                    cvt.writeInt32(newBankValue, cursor, 0xA000000 + (x % 8 + y * 8) * 0x800);
                                    cursor += 4;
                                }
                            bmp.Dispose();
                            if (bank.compressed)
                                newBankValue = SM64RAM.MIO0.encode_MIO0(newBankValue);
                            bank.ROM_End = bank.ROM_Start + (uint)newBankValue.Length;
                            Array.Copy(newBankValue, 0, EmulationState.instance.ROM, bank.ROM_Start, newBankValue.Length);
                            System.IO.File.WriteAllBytes(EmulationState.instance.ROMName, EmulationState.instance.ROM);
                            EmulationState.instance.banks[0xA] = new EmulationState.RAMBank(bank.ID, (int)bank.ROM_Start, (int)bank.ROM_End, bank.compressed);
                            Editor.projectSettings.banks.Add(bank);
                        }
                    }
            }
        }
    }
}
