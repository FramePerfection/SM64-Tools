using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64RAM
{
    public partial class PointerControl : UserControl
    {
        List<int> RAM_pointers = new List<int>();
        List<int> ROM_pointers = new List<int>();
        public EventHandler ValueChanged = new EventHandler((_, __) => {});

        public PointerControl()
        {
            InitializeComponent();
            listRAMPointers.KeyDown += listRAMPointers_KeyDown;
            listROMPointers.KeyDown += listROMPointers_KeyDown;
        }

        private void btnAddROMptr_Click(object sender, EventArgs e)
        {
            int ptr;
            if (!cvt.ParseIntHex(txtROMPointer.Text, out ptr))
            {
                MessageBox.Show("Invalid pointer value.");
                return;
            }
            ROM_pointers.Add(ptr);
            listROMPointers.DataSource = null;
            listROMPointers.DataSource = ROM_pointers;
            ValueChanged(this, new EventArgs());
        }

        private void btnAddRAMptr_Click(object sender, EventArgs e)
        {
            int ptr;
            if (!cvt.ParseIntHex(txtRAMPointer.Text, out ptr) || EmulationState.instance.banks[ptr >> 0x18] == null || EmulationState.instance.banks[ptr >> 0x18].compressed)
            {
                MessageBox.Show("Invalid pointer value.");
                return;
            }
            RAM_pointers.Add(ptr);
            listRAMPointers.DataSource = null;
            listRAMPointers.DataSource = RAM_pointers;
            ValueChanged(this, new EventArgs());
        }

        private void listROMPointers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listROMPointers.SelectedItem != null)
            {
                ROM_pointers.Remove((int)listROMPointers.SelectedItem);
                listROMPointers.DataSource = null;
                listROMPointers.DataSource = ROM_pointers;
                ValueChanged(this, new EventArgs());
            }
        }

        private void listRAMPointers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listRAMPointers.SelectedItem != null)
            {
                RAM_pointers.Remove((int)listRAMPointers.SelectedItem);
                listRAMPointers.DataSource = null;
                listRAMPointers.DataSource = RAM_pointers;
                ValueChanged(this, new EventArgs());
            }
        }

        public void SetROMPointers(int[] value)
        {
            ROM_pointers.Clear();
            if (value != null)
                ROM_pointers.AddRange(value);
            listROMPointers.DataSource = null;
            listROMPointers.DataSource = ROM_pointers;
            ValueChanged(this, new EventArgs());
        }

        public void SetRAMPointers(int[] value)
        {
            RAM_pointers.Clear();
            if (value != null)
                RAM_pointers.AddRange(value);
            listRAMPointers.DataSource = null;
            listRAMPointers.DataSource = RAM_pointers;
            ValueChanged(this, new EventArgs());
        }

        public int[] GetROMPointers() { return ROM_pointers.ToArray(); }

        public int[] GetRAMPointers() { return RAM_pointers.ToArray(); }

        public void WritePointers(int value)
        {
            byte[] ptrValue = new byte[] { (byte)(value >> 0x18), (byte)((value & 0xFF0000) >> 0x10), (byte)((value & 0xFF00) >> 0x8), (byte)(value & 0xFF) };
            foreach (int ptr in RAM_pointers)
                if (EmulationState.instance.AssertWrite(ptr, 4))
                    Array.Copy(ptrValue, 0, EmulationState.instance.ROM, EmulationState.instance.banks[ptr >> 0x18].ROMStart + (ptr & 0xFFFFFF), 4);
            foreach (int ptr in ROM_pointers)
                if (EmulationState.instance.AssertWrite(ptr, 4))
                    Array.Copy(ptrValue, 0, EmulationState.instance.ROM, ptr, 4);
        }
    }
}
