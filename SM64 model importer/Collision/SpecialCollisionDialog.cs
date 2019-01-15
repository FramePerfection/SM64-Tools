using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SM64ModelImporter
{
    public partial class SpecialCollisionDialog : Form
    {
        public enum KnownBoxTypes : short
        {
            Water = 0,
            Toxic = 0x32,
            Myst = 0x33
        }

        public SpecialBoxes cmd = new SpecialBoxes();
        SpecialCollisionBox selected = null;
        public SpecialCollisionDialog(SpecialBoxes copyFrom)
        {
            if (copyFrom != null)
                foreach (SpecialCollisionBox box in copyFrom.boxes)
                {
                    SpecialCollisionBox newBox = new SpecialCollisionBox();
                    newBox.type = box.type;
                    newBox.x1 = box.x1;
                    newBox.x2 = box.x2;
                    newBox.z1 = box.z1;
                    newBox.z2 = box.z2;
                    newBox.y = box.y;
                    cmd.boxes.Add(newBox);
                }
            InitializeComponent();
            lstBoxes.KeyDown += (_, __) =>
            {
                if (__.KeyCode == Keys.Delete && lstBoxes.SelectedIndex > -1)
                {
                    cmd.boxes.RemoveAt(lstBoxes.SelectedIndex);
                    lstBoxes.DataSource = null;
                    lstBoxes.DataSource = cmd.boxes;
                }
            };
            lstBoxes.DataSource = cmd.boxes;
        }

        private void btnAddBox_Click(object sender, EventArgs e)
        {
            cmd.boxes.Add(new SpecialCollisionBox());
            lstBoxes.DataSource = null;
            lstBoxes.DataSource = cmd.boxes;
            lstBoxes.SelectedIndex = cmd.boxes.Count - 1;
        }

        private void lstBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBoxes.SelectedIndex != -1)
            {
                selected = cmd.boxes[lstBoxes.SelectedIndex];
                int i = 0;
                foreach (string a in cmbType.Items)
                {
                    if (Enum.GetName(typeof(KnownBoxTypes), selected.type) == a)
                    {
                        cmbType.SelectedIndex = i;
                        break;
                    }
                    i++;
                }
                if (i >= cmbType.Items.Count)
                    cmbType.SelectedText = selected.type.ToString();
                numX1.Value = selected.x1;
                numZ1.Value = selected.z1;
                numX2.Value = selected.x2;
                numZ2.Value = selected.z2;
                numHeight.Value = selected.y;
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selected == null) return;
            bool wah = Enum.IsDefined(typeof(KnownBoxTypes), cmbType.Text);
            if (wah)
                selected.type = (short)Enum.Parse(typeof(KnownBoxTypes), cmbType.Text);
            else
            {
                short tryParse;
                if (short.TryParse(cmbType.SelectedText, out tryParse))
                    selected.type = tryParse;
            }
        }

        private void numX1_ValueChanged(object sender, EventArgs e)
        {
            if (selected != null) selected.x1 = (short)numX1.Value;
        }

        private void numHeight_ValueChanged(object sender, EventArgs e)
        {
            if (selected != null) selected.y = (short)numHeight.Value;
        }

        private void numX2_ValueChanged(object sender, EventArgs e)
        {
            if (selected != null) selected.x2 = (short)numX2.Value;
        }

        private void numZ2_ValueChanged(object sender, EventArgs e)
        {
            if (selected != null) selected.z2 = (short)numZ2.Value;
        }

        private void numZ1_ValueChanged(object sender, EventArgs e)
        {
            if (selected != null) selected.z1 = (short)numZ1.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
