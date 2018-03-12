using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SM64RAM;

namespace SM64ModelImporter
{
    public partial class DisplayListControl : UserControl, Importable, ReadWrite
    {
        public bool useCustomAddress { get { return false; } }
        DisplayList displayList = new DisplayList();
        int segmentOffset;
        int vertexOffset, commandOffset, totalSize;
        string sourceFileName = "";
        bool haltEvents = false;

        ParameterControl[] paramControls;
        ConversionSettings conversionSettings = new ConversionSettings();
        delegate void Importer(string fileName, ConversionSettings settings, ref DisplayList dsp, out Dictionary<string, TextureInfo> allMaterials, out string[] messages);

        public DisplayListControl()
        {
            InitializeComponent();
            blenderControl1.BlendModeChanged += (object sender, EventArgs e) =>
            {
                if (haltEvents) return;
                if (displayList != null)
                    displayList.renderstates.blendMode = blenderControl1.GetValues();
            };
            paramControls = new ParameterControl[] { paramColor, paramFogColor, paramFogIntensity };
            for (int k = 0; k < paramControls.Length; k++)
            {
                int i = k;
                paramControls[i].TypeChanged += () =>
                {
                    switch (paramControls[i].cmbType.SelectedIndex)
                    {
                        case 0: displayList.renderstates.getParameter[i] = null; break;
                        case 1: displayList.renderstates.getParameter[i] = GetParameterGlobal; break;
                        case 2: displayList.renderstates.getParameter[i] = GetParameterCustom; break;
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
            displayList.renderstates.blendMode = blenderControl1.GetValues();
            updateImportEnable(null, null);
        }

        private void btnConversionOptions_Click(object sender, EventArgs e)
        {
            if (conversionSettings.DoColorInterpretationDialog())
                LoadObj();
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

        public int PrepareForImport()
        {
            try
            {
                FileParser.Block old_settings = new FileParser.Block(this); //Restore settings for collision after loading
                LoadObj();
                LoadSettings(old_settings);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

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
                EmulationState.messages.AppendMessage("The selected bank 0x" + segment.ToString("X") + " is compressed in the ROM and can therefore not be altered.", "Error");
                return -1;
            }
            WriteVertices();
            WriteCommands();
            int offsetInBank = segmentOffset & 0xFFFFFF;
            for (int i = 0; i < displayList.materialValues.Length; i++)
            {
                cvt.writeInt32(bank.value, offsetInBank + 0x10 * i, displayList.materialValues[i].highShading);
                cvt.writeInt32(bank.value, offsetInBank + 0x10 * i + 8, displayList.materialValues[i].lowShading);
            }

            Array.Copy(bank.value, offsetInBank, EmulationState.instance.ROM, bank.ROMStart + offsetInBank, totalSize);

            pointerControl.WritePointers(segmentOffset + commandOffset);
            foreach (Subset subset in displayList.subsets)
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
            displayList.ClearSubsets();
            renderStateControl.Bind(displayList.renderstates);
            combinerStateControl.Bind(displayList.renderstates.combiner);
            Dictionary<string, TextureInfo> newMatLibrary;
            string[] messages;
            
            Importer import = null;
            if (sourceFileName.EndsWith(".obj"))
                import = ObjReader.Read;
            else if (sourceFileName.EndsWith(".dae"))
                import = ColladaImporter.Read;
            import(sourceFileName, conversionSettings, ref displayList, out newMatLibrary, out messages);
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
                EmulationState.messages.AppendMessage(lolol.ToString(), "Info");
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
            if (displayList != null)
                cursor += displayList.materialValues.Length * 0x10;
            vertexOffset = cursor - segmentOffset;
            UpdateVertexAndCommands();
        }

        void UpdateVertexAndCommands()
        {
            int cursor = segmentOffset + vertexOffset;
            if (displayList != null)
            {
                foreach (Subset subset in displayList.subsets)
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

            if (displayList == null)
            {
                txtRawCommands.Text = "No Displaylist loaded.";
                return;
            }
            displayList.BuildCommands(segmentOffset, layer);
            totalSize = commandOffset + displayList.commands.Length * 8;
            StringBuilder lolol = new StringBuilder();
            bool nonTriangle = true;
            foreach (DisplayList.Command cmd in displayList.commands)
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
            foreach (Subset subset in displayList.subsets)
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
            foreach (DisplayList.Command cmd in displayList.commands)
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
            block.SetInt("Color Interpretation", (int)conversionSettings.colorInterpretation);
            if (displayList != null)
            {
                block.SetInt("Blend modes", displayList.renderstates.blendMode);
                block.SetInt("Render states", displayList.renderstates.otherModesLow);
                block.SetInt("RCP Set", displayList.renderstates.RCPSet);
                block.SetInt("RCP Unset", displayList.renderstates.RCPUnset);
                block.SetInt("Combiner Low", (int)(displayList.renderstates.combiner.state & 0xFFFFFFFF));
                block.SetInt("Combiner High", (int)((displayList.renderstates.combiner.state >> 0x20) & 0xFFFFFF));
                block.SetDouble("Texture Scale X", displayList.renderstates.textureScaleX);
                block.SetDouble("Texture Scale Y", displayList.renderstates.textureScaleY);
            }
            if (this.textureControl.materialLibrary != null)
            foreach (KeyValuePair<string, TextureInfo> tex in this.textureControl.materialLibrary)
                if (tex.Value.addressX != TextureInfo.AddressMode.G_TX_WRAP || tex.Value.addressY != TextureInfo.AddressMode.G_TX_WRAP || tex.Value.stRAMPointers.Length + tex.Value.stROMPointers.Length > 0)
                    block.SetBlock(tex.Key, new FileParser.Block(tex.Value));
        }

        public void LoadSettings(FileParser.Block block)
        {
            sourceFileName = block.GetString("Obj File");
            conversionSettings.colorInterpretation = (ConversionSettings.ColorInterpretation)block.GetInt("Color Interpretation", false);
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


            if (displayList != null)
            {
                displayList.renderstates.blendMode = block.GetInt("Blend modes");
                displayList.renderstates.otherModesLow = block.GetInt("Render states");
                displayList.renderstates.RCPSet = block.GetInt("RCP Set", false);
                displayList.renderstates.RCPUnset = block.GetInt("RCP Unset", false);
                displayList.renderstates.combiner.state = ((long)(block.GetInt("Combiner High") & 0xFFFFFFFF) << 0x20) | ((long)block.GetInt("Combiner Low") & 0xFFFFFFFF);
                displayList.renderstates.textureScaleX = block.GetDouble("Texture Scale X");
                displayList.renderstates.textureScaleY = block.GetDouble("Texture Scale Y");

                renderStateControl.Bind(displayList.renderstates);
                combinerStateControl.Bind(displayList.renderstates.combiner);
                blenderControl1.SetValues(displayList.renderstates.blendMode);
            }
            updateImportEnable(null, null);
        }

        #endregion

    }
}
