using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SM64RAM
{
    public partial class RemapBankDialog : Form
    {
        EmulationState state;
        public EmulationState.RAMBank selectedBank { get; private set; }
        public int start { get; private set; }
        public int length { get; private set; }

        public RemapBankDialog(EmulationState state)
        {
            InitializeComponent();
            this.state = state;
            foreach (EmulationState.RAMBank bank in state.banks)
                if (bank != null)
                    cmbBank.Items.Add(bank);
            btnOK.Enabled = false;
        }

        private void cmbBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBank = cmbBank.SelectedItem as EmulationState.RAMBank;
            btnOK.Enabled = selectedBank != null;
            if (selectedBank == null) return;
            pointerControl1.SetROMPointers(selectedBank.ptrsROM.ToArray());
            pointerControl1.SetRAMPointers(selectedBank.ptrsRAM.ToArray());
            txtLength.Enabled = !selectedBank.compressed;
            txtROMStart.Text = "0x" + selectedBank.ROMStart.ToString("X");
            txtLength.Text = "0x" + (selectedBank.ROMEnd - selectedBank.ROMStart).ToString("X");
            btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Hide();
        }

        private void txtROMStart_TextChanged(object sender, EventArgs e)
        {
            int num;
            if (cvt.ParseIntHex(txtROMStart.Text.Substring(2), out num))
                start = num;
        }

        private void txtLength_TextChanged(object sender, EventArgs e)
        {
            int num;
            if (cvt.ParseIntHex(txtLength.Text.Substring(2), out num))
                length  = num;
        }
    }
}
