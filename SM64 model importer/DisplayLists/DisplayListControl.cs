using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SM64RAM;

namespace SM64_model_importer
{
    public partial class DisplayListControl : UserControl, Importable, ReadWrite
    {
        public bool useCustomAddress { get { return false; } }
        DisplayList newDsp = new DisplayList();
        int segmentOffset;
        int vertexOffset, commandOffset, totalSize;
        string sourceFileName = "";
        bool haltEvents = false;

        ParameterControl[] paramControls;
        delegate void Importer(string fileName, ref DisplayList dsp, out Dictionary<string, TextureInfo> allMaterials, out string[] messages);

        public DisplayListControl()
        {
            InitializeComponent();
            blenderControl1.BlendModeChanged += (object sender, EventArgs e) =>
            {
                if (haltEvents) return;
                if (newDsp != null)
                    newDsp.renderstates.blendMode = blenderControl1.GetValues();
            };
            paramControls = new ParameterControl[] { paramColor, paramFogColor, paramFogIntensity };
            for (int k = 0; k < paramControls.Length; k++)
            {
                int i = k;
                paramControls[i].TypeChanged += () =>
                {
                    switch (paramControls[i].cmbType.SelectedIndex)
                    {
                        case 0: newDsp.renderstates.getParameter[i] = null; break;
                        case 1: newDsp.renderstates.getParameter[i] = GetParameterGlobal; break;
                        case 2: newDsp.renderstates.getParameter[i] = GetParameterCustom; break;
                    }
                };
            }
        }

        #region GUI events

        private void btnLoadObj_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Wavefront objet files (*.obj)|*.obj|Collada files (*.dae)|*.dae";
            if (dlg.ShowDialog() != DialogResult.OK) return;
            sourceFileName = dlg.FileName;
            LoadObj();
            newDsp.renderstates.blendMode = blenderControl1.GetValues();
            updateImportEnable(null, null);
        }

        private void updateImportEnable(object sender, EventArgs e)
        {
            UpdateAll();
        }

        private void btnSavePreset_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Display List presets|*.dl";
            if (sfd.ShowDialog() != DialogResult.OK) return;
            System.IO.StreamWriter writer = new StreamWriter(sfd.FileName);
            FileParser.Block root = new FileParser.Block(this);
            SaveSettings(root);
            root.Write(writer);
            writer.Close();
        }

        private void btnLoadPreset_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Display List presets|*.dl";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            LoadSettings(new FileParser.Block(ofd.FileName));
        }

        #endregion

        #region Import Functions

        public int Import(int segmentOffset)
        {
            this.segmentOffset = segmentOffset;
            UpdateAll();
            int segment = segmentOffset >> 0x18;
            EmulationState.RAMBank bank = EmulationState.instance.banks[segment];
            if (!EmulationState.instance.AssertRead(segmentOffset, totalSize))
                return -1;
            if (bank.compressed)
            {
                MessageBox.Show("The selected bank 0x" + segment.ToString("X") + " is compressed in the ROM and can therefore not be altered.");
                return -1;
            }
            WriteVertices();
            WriteCommands();
            int offsetInBank = segmentOffset & 0xFFFFFF;
            for (int i = 0; i < newDsp.materialValues.Length; i++)
            {
                cvt.writeInt32(bank.value, offsetInBank + 0x10 * i, newDsp.materialValues[i].highShading);
                cvt.writeInt32(bank.value, offsetInBank + 0x10 * i + 8, newDsp.materialValues[i].lowShading);
            }

            Array.Copy(bank.value, offsetInBank, EmulationState.instance.ROM, bank.ROMStart + offsetInBank, totalSize);

            pointerControl.WritePointers(segmentOffset + commandOffset);
            foreach (Subset subset in newDsp.subsets)
            {
                int value = subset.Texture.scrollingTextureRelativePointer + segmentOffset + commandOffset;
                byte[] ptrValue = new byte[] { (byte)(value >> 0x18), (byte)((value & 0xFF0000) >> 0x10), (byte)((value & 0xFF00) >> 0x8), (byte)(value & 0xFF) };
                foreach (int ptr in subset.Texture.stRAMPointers)
                    if (EmulationState.instance.AssertWrite(ptr, 4))
                        Array.Copy(ptrValue, 0, EmulationState.instance.ROM, EmulationState.instance.banks[ptr >> 0x18].ROMStart + (ptr & 0xFFFFFF), 4);
                foreach (int ptr in subset.Texture.stROMPointers)
                    if (EmulationState.instance.AssertWrite(ptr, 4))
                        Array.Copy(ptrValue, 0, EmulationState.instance.ROM, ptr, 4);
            }
            return segmentOffset + totalSize;
        }

        void LoadObj()
        {
#if !DEBUG
            try
            {
#endif
            newDsp.ClearSubsets();
            renderStateControl.Bind(newDsp.renderstates);
            combinerStateControl.Bind(newDsp.renderstates.combiner);
            Dictionary<string, TextureInfo> newMatLibrary;
            string[] messages;
            
            Importer import = null;
            if (sourceFileName.EndsWith(".obj"))
                import = ObjReader.Read;
            else if (sourceFileName.EndsWith(".dae"))
                import = ColladaImporter.Read;
            import(sourceFileName, ref newDsp, out newMatLibrary, out messages);
            textureControl.materialLibrary = newMatLibrary;
            textureControl.Invalidate();
            lblObjFileName.Text = Path.GetFileName(sourceFileName);
            if (messages.Length > 0)
            {
                StringBuilder lolol = new StringBuilder();
                for (int i = 0; i < messages.Length; i++)
                {
                    lolol.Append(messages[i]);
                    if (i < messages.Length - 1)
                        lolol.Append("\n");
                }
                MessageBox.Show(lolol.ToString());
            }
#if !DEBUG
            }
            catch
            {
                lblObjFileName.Text = "<Error>";

            }
#endif
        }

        void UpdateAll()
        {
            int cursor = segmentOffset;
            if (newDsp != null)
                cursor += newDsp.materialValues.Length * 0x10;
            vertexOffset = cursor - segmentOffset;
            UpdateVertexAndCommands();
        }

        void UpdateVertexAndCommands()
        {
            int cursor = segmentOffset + vertexOffset;
            if (newDsp != null)
            {
                foreach (Subset subset in newDsp.subsets)
                    foreach (TrianglePatch patch in subset.Patches)
                    {
                        patch.segmentedPointer = cursor;
                        cursor += patch.GetSizeInBytes();
                    }
            }
            commandOffset = cursor - segmentOffset;
            UpdateCommands();
        }

        void UpdateCommands()
        {
            int layer = (int)numLayer.Value;
            if (layer == 2 || layer == 3) layer = 1;

            if (newDsp == null)
            {
                txtRawCommands.Text = "No Displaylist loaded.";
                return;
            }
            newDsp.BuildCommands(segmentOffset, layer);
            totalSize = commandOffset + newDsp.commands.Length * 8;
            StringBuilder lolol = new StringBuilder();
            bool nonTriangle = true;
            foreach (DisplayList.Command cmd in newDsp.commands)
            {
                if (cmd.values[0] == 0x4 || cmd.values[0] == 0xBF)
                {
                    if (nonTriangle) lolol.Append("[Triangles]\n\n");
                    nonTriangle = false;
                }
                else
                {
                    lolol.Append(cmd.ToString() + "\n");
                    nonTriangle = true;
                }
            }
            txtRawCommands.Text = lolol.ToString();
        }

        void WriteVertices()
        {
            ;
            foreach (Subset subset in newDsp.subsets)
            {
                int baseMultiplierS = subset.Texture.baseMultiplierS;
                int baseMultiplierT = subset.Texture.baseMultiplierT;
                foreach (TrianglePatch patch in subset.Patches)
                    patch.WriteBytes(baseMultiplierS, baseMultiplierT);
            }
        }

        void WriteCommands()
        {
            int segment = segmentOffset >> 0x18;
            EmulationState.RAMBank target = EmulationState.instance.banks[segment];
            if (target == null)
                throw new Exception("Bad commands");

            int cursor = (segmentOffset & 0xFFFFFF) + commandOffset;
            foreach (DisplayList.Command cmd in newDsp.commands)
            {
                Array.Copy(cmd.values, 0, target.value, cursor, cmd.values.Length);
                cursor += cmd.values.Length;
            }
        }

        DisplayList.Command GetParameterCustom(RenderStates.Parameter param)
        {
            return RenderStates.CreateParameterCommand(param, paramControls[(int)param].customValue);
        }

        DisplayList.Command GetParameterGlobal(RenderStates.Parameter param)
        {
            return new DisplayList.Command(0x06, 0, Main.instance.globalsControl.GetParameterAddress(param));
        }

        #endregion

        #region Save/Load Settings

        public void SaveSettings(FileParser.Block block)
        {
            block.SetBool("Use Custom Address", useCustomAddress);
            if (useCustomAddress)
                block.SetInt("Custom Address", segmentOffset);
            block.SetIntArray("ROM Pointers", pointerControl.GetROMPointers());
            block.SetIntArray("RAM Pointers", pointerControl.GetRAMPointers());
            block.SetInt("Layer", (int)numLayer.Value);

            for (int i = 0; i < paramControls.Length; i++)
            {
                block.SetInt(paramControls[i].ParameterName, (int)paramControls[i].cmbType.SelectedIndex);
                if (paramControls[i].cmbType.SelectedIndex == paramControls[i].cmbType.Items.Count - 1)
                    block.SetInt("Custom " + paramControls[i].ParameterName, paramControls[i].customValue);
            }

            block.SetString("Obj File", sourceFileName);
            if (newDsp != null)
            {
                block.SetInt("Blend modes", newDsp.renderstates.blendMode);
                block.SetInt("Render states", newDsp.renderstates.otherModesLow);
                block.SetInt("RCP bits", newDsp.renderstates.RCPBits);
                block.SetInt("Combiner Low", (int)(newDsp.renderstates.combiner.state & 0xFFFFFFFF));
                block.SetInt("Combiner High", (int)((newDsp.renderstates.combiner.state >> 0x20) & 0xFFFFFF));
                block.SetDouble("Texture Scale X", newDsp.renderstates.textureScaleX);
                block.SetDouble("Texture Scale Y", newDsp.renderstates.textureScaleY);
            }
            foreach (KeyValuePair<string, TextureInfo> tex in this.textureControl.materialLibrary)
                if (tex.Value.addressX != TextureInfo.AddressMode.G_TX_WRAP || tex.Value.addressY != TextureInfo.AddressMode.G_TX_WRAP || tex.Value.stRAMPointers.Length + tex.Value.stROMPointers.Length > 0)
                    block.SetBlock(tex.Key, new FileParser.Block(tex.Value));
        }

        public void LoadSettings(FileParser.Block block)
        {
            sourceFileName = block.GetString("Obj File");
            if (sourceFileName != "")
                LoadObj();
            pointerControl.SetROMPointers(block.GetIntArray("ROM Pointers"));
            pointerControl.SetRAMPointers(block.GetIntArray("RAM Pointers"));

            numLayer.Value = block.GetInt("Layer");
            for (int i = 0; i < paramControls.Length; i++)
            {
                paramControls[i].cmbType.SelectedIndex = block.GetInt(paramControls[i].ParameterName, false);
                if (paramControls[i].cmbType.SelectedIndex == paramControls[i].cmbType.Items.Count - 1)
                    paramControls[i].customValue = block.GetInt("Custom " + paramControls[i].ParameterName, false);
            }

            if (textureControl.materialLibrary != null)
                foreach (KeyValuePair<string, TextureInfo> tex in textureControl.materialLibrary)
                {
                    FileParser.Block b = block.GetBlock(tex.Key, false);
                    if (b != null) tex.Value.LoadSettings(b);
                }


            if (newDsp != null)
            {
                newDsp.renderstates.blendMode = block.GetInt("Blend modes");
                newDsp.renderstates.otherModesLow = block.GetInt("Render states");
                newDsp.renderstates.RCPBits = block.GetInt("RCP bits");
                newDsp.renderstates.combiner.state = ((long)(block.GetInt("Combiner High") & 0xFFFFFFFF) << 0x20) | ((long)block.GetInt("Combiner Low") & 0xFFFFFFFF);
                newDsp.renderstates.textureScaleX = block.GetDouble("Texture Scale X");
                newDsp.renderstates.textureScaleY = block.GetDouble("Texture Scale Y");

                renderStateControl.Bind(newDsp.renderstates);
                combinerStateControl.Bind(newDsp.renderstates.combiner);
                blenderControl1.SetValues(newDsp.renderstates.blendMode);
            }
            updateImportEnable(null, null);
        }

        #endregion
    }
}
