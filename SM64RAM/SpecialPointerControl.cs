using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64RAM
{
    public partial class SpecialPointerControl : UserControl
    {
        SpecialPointer[] pointerSource;
        List<SpecialPointer> pointers = new List<SpecialPointer>();
        public SpecialPointer[] GetPointers() { return pointers.ToArray(); }

        public SpecialPointerControl()
        {
            InitializeComponent();
            listSpecialPointers.KeyDown += listSpecialPointers_KeyDown;
        }

        public void SetPointerSource(SpecialPointer[] pointerSource)
        {
            this.pointerSource = pointerSource;
            cmbSpecialPointers.DataSource = null;
            cmbSpecialPointers.DataSource = pointerSource;
        }

        public void SetPointers(SpecialPointer[] value)
        {
            pointers.Clear();
            pointers.AddRange(value);
            listSpecialPointers.DataSource = pointers;
        }

        public SpecialPointer PointerByString(string identifierString)
        {
            foreach (SpecialPointer obj in pointerSource)
                if (obj.ToString().Trim() == identifierString)
                    return obj;
            return null;
        }

        public void WritePointers(int value)
        {
            byte[] ptrValue = new byte[] { (byte)(value >> 0x18), (byte)((value & 0xFF0000) >> 0x10), (byte)((value & 0xFF00) >> 0x8), (byte)(value & 0xFF) };
            foreach (SpecialPointer ptrContainer in pointers)
            {
                int ptr = ptrContainer.GetSegmentedValue();
                if (EmulationState.instance.AssertWrite(ptr, 4))
                    Array.Copy(ptrValue, 0, EmulationState.instance.ROM, EmulationState.instance.banks[ptr >> 0x18].ROMStart + (ptr & 0xFFFFFF), 4);
            }
        }

        private void btnAddRAMptr_Click(object sender, EventArgs e)
        {
            SpecialPointer p = cmbSpecialPointers.SelectedItem as SpecialPointer;
            if (p == null) return;
            if (!pointers.Contains(p))
            {
                pointers.Add(p);
                listSpecialPointers.DataSource = null;
                listSpecialPointers.DataSource = pointers;
            }
        }

        private void listSpecialPointers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listSpecialPointers.SelectedItem != null)
            {
                pointers.Remove((SpecialPointer)listSpecialPointers.SelectedItem);
                listSpecialPointers.DataSource = null;
                listSpecialPointers.DataSource = pointers;
            }
        }
    }
}
