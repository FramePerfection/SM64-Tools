using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SM64RAM;
using System.Text.RegularExpressions;

namespace SM64_model_importer
{
    public interface Importable
    {
        bool useCustomAddress { get; }
        int PrepareForImport();
        int Import(int segmentedAddress);
    }

    public partial class Main : Form, ReadWrite
    {
        public static Main instance;

        public TextureLibrary textureLibrary = new TextureLibrary();

        public int baseOffset { get { return segmentedAddress; } }

        List<DisplayListControl> displayLists = new List<DisplayListControl>();
        List<CollisionControl> collisionMaps = new List<CollisionControl>();
        List<int> runLevelScripts = new List<int>();
        int segmentedAddress = 0x0e000000;

        bool loadingLevelScripts = false;

        static Main()
        {
            CollisionControl.Init();
        }

        public Main()
        {
            instance = this;
            InitializeComponent();
            FormClosing += main_Close;
            EmulationState.instance.ROMLoaded += () => CheckImportValid();
            RAMControl.RunLevelScriptFinished += (int address) =>
            {
                if (!loadingLevelScripts)
                    runLevelScripts.Add(address);
            };
            RAMControl.ClearBanks += () => runLevelScripts.Clear();
            EmulationState.instance.ROMLoaded += RunLevelScripts;
        }

        void RunLevelScripts()
        {
            loadingLevelScripts = true;
            foreach (int script in runLevelScripts)
                RAMControl.RunLevelScript(script);
            loadingLevelScripts = false;
        }

        #region GUI Events

        private void main_Close(object sender, FormClosingEventArgs e)
        {
            LevelScriptReader.SaveLog("log.txt");
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!EmulationState.instance.AssertWrite(segmentedAddress, 1))
                return;

            EmulationState.instance.RefreshROM();
            int numObjects = 0;
            foreach (TabPage page in tabImports.TabPages)
                foreach (Control c in page.Controls[0].Controls)
                {
                    Importable import = c as Importable;
                    if (import != null)
                    {
                        if (import.PrepareForImport() == -1)
                        {
                            EmulationState.messages.AppendMessage("Import failed.", "Error");
                            return;
                        }
                        numObjects++;
                    }
                }

            StringBuilder log = new StringBuilder();
            int cursor = segmentedAddress;
            cursor = globalsControl.Import(cursor);

            foreach (KeyValuePair<string, TextureImage> texture in textureLibrary.textures)
            {
                texture.Value.segmentOffset = cursor;
                texture.Value.WriteBytes();
                cursor += texture.Value.GetSizeInBytes();
            }
            int offsetInBank = segmentedAddress & 0xFFFFFF;
            EmulationState.RAMBank bank = EmulationState.instance.banks[segmentedAddress >> 0x18];
            Array.Copy(bank.value, offsetInBank, EmulationState.instance.ROM, bank.ROMStart + offsetInBank, cursor - segmentedAddress);

            foreach (TabPage page in tabImports.TabPages)
                foreach (Control c in page.Controls[0].Controls)
                {
                    Importable import = c as Importable;
                    if (import != null)
                    {
                        int newCursor = import.Import(cursor);
                        if (newCursor == -1)
                        {
                            EmulationState.messages.AppendMessage("Import failed.", "Error");
                            return;
                        }
                        else if (newCursor != 0)
                            cursor = newCursor;
                    }
                }
            File.WriteAllBytes(EmulationState.instance.ROMName, EmulationState.instance.ROM);
            EmulationState.messages.AppendMessage("Imported " + numObjects + " objects.", "Info");
        }

        private void btnAddDisplayList_Click(object sender, EventArgs e)
        {
            AddDisplayList(new DisplayListControl());
        }

        private void btnAddCollision_Click(object sender, EventArgs e)
        {
            AddCollision(new CollisionControl());
        }

        private void txtBaseOffset_TextChanged(object sender, EventArgs e)
        {
            CheckImportValid();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Display List presets|*.sm64import";
            if (sfd.ShowDialog() != DialogResult.OK) return;
            System.IO.StreamWriter writer = new StreamWriter(sfd.FileName);
            FileParser.Block root = new FileParser.Block(this);
            SaveSettings(root);
            root.Write(writer);
            writer.Close();
        }

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Display List presets|*.sm64import";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FileParser.Block b = new FileParser.Block(ofd.FileName);
            LoadSettings(b);
        }

        #endregion

        void CheckImportValid()
        {
            bool enable;
            int newValue;
            if (enable = cvt.ParseIntHex(txtBaseOffset.Text, out newValue))
                segmentedAddress = newValue;
            btnImport.Enabled = enable & EmulationState.instance.AssertROM(false);
        }

        void AddTabPage(Control ctrl, string name)
        {
            tabImports.SuspendLayout();
            TabPage newTab = new TabPage();
            ctrl = new ObjectTabPage(ctrl, newTab, name);
            newTab.Controls.Add(ctrl);
            int pageCount = tabImports.TabPages.Count;
            tabImports.ResumeLayout();
            tabImports.TabPages.Insert(pageCount - 1, newTab);
            tabImports.SelectedIndex = pageCount - 1;
        }

        void AddDisplayList(DisplayListControl ctrl)
        {
            AddTabPage(ctrl, "Display List");
            displayLists.Add(ctrl);
        }

        void AddCollision(CollisionControl ctrl)
        {
            AddTabPage(ctrl, "Collision");
            collisionMaps.Add(ctrl);
        }

        #region Save/Load Settings

        public void SaveSettings(FileParser.Block block)
        {
            block.SetString("ROM", EmulationState.instance.ROMName);
            block.SetIntArray("Run level scripts", runLevelScripts.ToArray());
            block.SetInt("Base Offset", segmentedAddress);
            block.SetInt("Import Scale", (int)Settings.importScale);

            block.SetBlock("Globals", new FileParser.Block(globalsControl));

            block.SetBlock("Textures", new FileParser.Block(textureLibrary));

            //Write Display List data
            block.SetInt("NumDisplayLists", displayLists.Count, false);
            int i = 0;
            foreach (ReadWrite dl in displayLists)
                block.SetBlock("DisplayList" + i++, new FileParser.Block(dl));

            //Write Collision Map data
            block.SetInt("NumCollisionMaps", collisionMaps.Count, false);
            i = 0;
            foreach (ReadWrite dl in collisionMaps)
                block.SetBlock("CollisionMap" + i++, new FileParser.Block(dl));
        }

        public void LoadSettings(FileParser.Block block)
        {
            string fileName = block.GetString("ROM");
            if (!System.IO.File.Exists(fileName))
            {
                string prompt = "File " + fileName + " does not exist anymore. Do you want to load another ROM?";
                if (fileName != "" && MessageBox.Show(prompt, "ROM not found.", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes) return;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "z64 ROMs|*.z64";
                if (SM64RAM.EmulationState.instance.ROMName != "")
                    fileName = SM64RAM.EmulationState.instance.ROMName;
                else
                {
                    if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
                    fileName = dlg.FileName;
                }
            }
            RAMControl.LoadROM(fileName);
            runLevelScripts.Clear();
            if (SM64RAM.EmulationState.instance.ROM == null)
            {
                EmulationState.messages.AppendMessage("An error occured while loading the ROM. Make sure no other programs are using the ROM file right now and try again.");
                return;
            }

            segmentedAddress = block.GetInt("Base Offset", false);
            txtBaseOffset.Text = segmentedAddress.ToString("X8");
            Settings.importScale = block.GetInt("Import Scale", false);
            if (Settings.importScale == 0) Settings.importScale = 1000;
            numScale.Value = Math.Max((int)numScale.Minimum, Math.Min((int)numScale.Maximum, (int)Settings.importScale));

            FileParser.Block globalBlock = block.GetBlock("Globals", false);
            if (globalBlock != null)
                globalsControl.LoadSettings(globalBlock);

            //Apply patches if necessary / desired
            string patchString = block.GetString("Patches", false);
            if (patchString != "" && MessageBox.Show("Apply level patches?", "Patch ROM", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                PatchEngine.Run(patchString, Path.GetDirectoryName(block.fileName));

            runLevelScripts.AddRange(block.GetIntArray("Run level scripts"));
            RunLevelScripts();

            textureLibrary.textures.Clear();
            displayLists.Clear();
            collisionMaps.Clear();
            SuspendLayout();
            tabImports.TabPages.Clear();
            tabImports.TabPages.Add(tabPagePlus);

            //Load Display List Data
            int numDls = block.GetInt("NumDisplayLists");
            for (int i = 0; i < numDls; i++)
            {
                DisplayListControl newControl = new DisplayListControl();
                newControl.PerformLayout();
                newControl.LoadSettings(block.GetBlock("DisplayList" + i));
                AddDisplayList(newControl);
            }

            textureLibrary.LoadSettings(block.GetBlock("Textures", false));

            //Load Collision Map Data
            int numCMaps = block.GetInt("NumCollisionMaps");
            for (int i = 0; i < numCMaps; i++)
            {
                CollisionControl newControl = new CollisionControl();
                newControl.PerformLayout();
                newControl.LoadSettings(block.GetBlock("CollisionMap" + i));
                AddCollision(newControl);
            }
            ResumeLayout();
        }

        #endregion

        private void numScale_ValueChanged(object sender, EventArgs e)
        {
            Settings.importScale = (float)numScale.Value;
        }
    }
}
