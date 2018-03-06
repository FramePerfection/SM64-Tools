using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SM64RAM
{
    public partial class RAMControl : UserControl
    {
        public delegate void RunLevelScriptHandler(int levelScriptAddress);
        public delegate void Action();
        public event RunLevelScriptHandler RunLevelScriptFinished;
        public event Action ClearBanks;

        public RAMControl()
        {
            InitializeComponent();
        }

        public void LoadROM(string fileName)
        {
            lblROMName.Text = Path.GetFileName(fileName);
            try
            {
                EmulationState.instance.LoadROM(fileName);
            }
            catch
            {
                EmulationState.instance.ROM = null;
                EmulationState.instance.banks = null;
                lblROMName.Text = "<Error>";
            }
        }

        public void RunLevelScript(int offset)
        {
            LevelScriptReader.ReadFrom(offset);
            if (RunLevelScriptFinished != null) RunLevelScriptFinished(offset);

            StringBuilder loadedBanksBuilder = new StringBuilder();
            int charBreak = 0;
            for (int i = 0; i < EmulationState.instance.banks.Length; i++)
                if (EmulationState.instance.banks[i] != null)
                    loadedBanksBuilder.Append(((charBreak++ % 3 == 0) ? '\n' : ' ') + "0x" + i.ToString("X") + ": 0x"
                    + EmulationState.instance.banks[i].ROMStart.ToString("X") + " - 0x" + EmulationState.instance.banks[i].ROMEnd.ToString("X") + ";");
            lblBanks.Text = "Loaded Banks:" + loadedBanksBuilder.ToString();
        }

        private void btnLoadROM_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "ROM files (*.z64)|*.z64";
            if (dlg.ShowDialog() != DialogResult.OK) return;
            LoadROM(dlg.FileName);
        }

        private void btnRunLevelScript_Click(object sender, EventArgs e)
        {
            if (!EmulationState.instance.AssertROM()) return;
            int offset = 0;
            if (!cvt.ParseIntHex(txtROMOffset.Text, out offset))
            {
                MessageBox.Show("Invalid offset!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RunLevelScript(offset);
        }

        private void btnClearRAMBanks_Click(object sender, EventArgs e)
        {
            if (ClearBanks != null) ClearBanks();
                EmulationState.instance.ClearBanks();
        }

        private void btbRemapBank_Click(object sender, EventArgs e)
        {
            RemapBankDialog dlg = new RemapBankDialog(EmulationState.instance);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
               dlg.selectedBank.Remap(dlg.start, dlg.length);
                File.WriteAllBytes(EmulationState.instance.ROMName + ".test", EmulationState.instance.ROM);
            }
            dlg.Dispose();
        }
    }
}
