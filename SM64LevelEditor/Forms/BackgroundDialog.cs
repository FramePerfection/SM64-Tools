using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace SM64LevelEditor
{
    public partial class BackgroundDialog : Form
    {
        public string imageFileName { get; private set; }
        public BankDescription selectedBank { get {             return groupNew.Enabled ? newBank : selectedWrapper.bank; } }
        BankWrapper selectedWrapper;
        BankDescription newBank = new BankDescription();
        struct BankWrapper
        {
            public BankDescription bank;
            public BankWrapper(BankDescription bank) { this.bank = bank; }
            public override string ToString()
            {
                return bank.name + " (0x" + bank.ROM_Start.ToString("X") + " - 0x" + bank.ROM_End.ToString("X") + ")";
            }
        }
        public BackgroundDialog()
        {
            InitializeComponent();
            if (Editor.currentLevel == null) return;
            foreach (BankDescription bank in Editor.projectSettings.banks)
                if (bank.type == BankDescription.BankType.Background)
                    cmbSelectedBank.Items.Add(new BankWrapper(bank));
            cmbSelectedBank.Items.Add("<new>");
            newBank.ID = 0xA;
        }

        private void txtNewROMAddress_TextChanged(object sender, EventArgs e)
        {
            uint newAddress;
            if (uint.TryParse(txtNewROMAddress.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out newAddress))
                newBank.ROM_Start = newAddress;
        }

        private void cmbSelectedBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool newBackground = cmbSelectedBank.SelectedIndex == cmbSelectedBank.Items.Count - 1;
            if (!newBackground)
            {
                selectedWrapper = (BankWrapper)cmbSelectedBank.SelectedItem;
                groupNew.Enabled = false;
            }
            else
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    cmbSelectedBank.SelectedItem = selectedWrapper;
                    return;
                }
                imageFileName = dlg.FileName;
                groupNew.Enabled = true;

            }
        }

        private void txtNewName_TextChanged(object sender, EventArgs e)
        {
            newBank.name = txtNewName.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void chkCompress_CheckedChanged(object sender, EventArgs e)
        {
            newBank.compressed = chkCompress.Checked;
        }

        private void numericArgument_ValueChanged(object sender, EventArgs e)
        {
            newBank.arg = (byte)numericArgument.Value;
        }
    }
}
