using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SM64RAM
{
    public class EmulationState
    {
        public class RAMBank
        {
            public int ROMStart;
            public int ROMEnd { get { return original == null ? -1 : ROMStart + original.Length; } }
            byte[] original;
            public byte[] value;
            public bool compressed { get; private set; }
            public List<int> ptrsROM = new List<int>();
            public List<int> ptrsRAM = new List<int>();
            int index;
            public RAMBank(int index, int ROMStart, int ROMEnd, bool decompress = false)
            {
                this.index = index;
                compressed = decompress;
                this.ROMStart = ROMStart;
                if (ROMStart < 0) return;
                original = new byte[ROMEnd - ROMStart];
                Array.Copy(EmulationState.instance.ROM, ROMStart, original, 0, original.Length);
                if (decompress)
                    value = MIO0.decode_MIO0(original, 0);
                else
                    value = original;
            }

            public void Remap(int start, int length)
            {
                foreach (int ptr in ptrsRAM)
                {
                    int bank = ptr >> 0x18;
                    if (EmulationState.instance.AssertWrite(ptr, 0xC)) return;
                }
                int end = start + length;
                foreach (int ptr in ptrsROM)
                {
                    int lol = ptr;
                    cvt.writeInt32(EmulationState.instance.ROM, ptr + 4, start);
                    cvt.writeInt32(EmulationState.instance.ROM, ptr + 8, end);
                }
                foreach (int ptr in ptrsRAM)
                {
                    int bank = ptr >> 0x18;
                    int lol = EmulationState.instance.banks[bank].ROMStart + (ptr & 0xFFFFFF);
                    cvt.writeInt32(EmulationState.instance.ROM, lol + 4, start);
                    cvt.writeInt32(EmulationState.instance.ROM, lol + 8, end);
                }
                this.ROMStart = start;
                Array.Copy(original, 0, EmulationState.instance.ROM, ROMStart, length);
            }

            public override string ToString()
            {
                return "0x" + index.ToString("X2") + " (0x" + ROMStart.ToString("X") + " - 0x" + ROMEnd.ToString("X") + ")";
            }
        }

        public static EmulationState instance = new EmulationState();
        public byte[] ROM;
        public RAMBank[] banks = new RAMBank[0x25];
        public string ROMName = "";

        public delegate void Action();
        public event Action ROMLoaded;
        public delegate void BankLoadedAction(int bank);
        public event BankLoadedAction BankLoaded;

        private EmulationState() { }

        #region Asserts

        public bool AssertROM(bool showError = true)
        {
            if (ROM == null)
                if (showError)
                    MessageBox.Show("No ROM loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return ROM != null;
        }

        public bool AssertRead(int value, int length = 1, bool showError = true)
        {
            if (!AssertROM()) return false;
            int segment = value >> 0x18;
            if (segment >= banks.Length || banks[segment] == null || banks[segment].value == null)
            {
                if (showError)
                    MessageBox.Show(segment >= banks.Length ? "Invalid Bank!" : "Bank 0x" + segment.ToString("X") + " not loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if ((value & 0xFFFFFF) > banks[segment].value.Length - length)
            {
                if (showError)
                    MessageBox.Show("Bank 0x" + segment.ToString("X") + " is too short.\nBank length was " + banks[segment].value.Length + ", but requested length was " + (value + length).ToString("X") + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public bool AssertWrite(int address, int length, bool showError = true)
        {
            if (!AssertRead(address, length)) return false;
            int segment = address >> 0x18;
            if (banks[segment].compressed)
            {
                if (showError)
                    MessageBox.Show("Bank 0x" + segment + " is compressed and can therefore not be altered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        #endregion

        public void LoadROM(string fileName)
        {
            ROM = PrepareROM.Normalize(fileName);
            banks = new EmulationState.RAMBank[0x25];
            ROMName = fileName;
            if (ROMLoaded != null)
                ROMLoaded();
        }

        public void RefreshROM()
        {
            if (File.Exists(ROMName))
                ROM = File.ReadAllBytes(ROMName);
        }

        public void ClearBanks()
        {
            banks = new EmulationState.RAMBank[0x25];
        }

        public void LoadBank(int bank, int ROMStart, int ROMEnd, bool decompress)
        {
            if (bank >= banks.Length)
            {
                MessageBox.Show("Bank 0x" + bank.ToString("X") + " is too high!", "Error");
                return;
            }
            if (!AssertROM()) return;
            if (ROMEnd > ROM.Length || ROMEnd < ROMStart)
            {
                MessageBox.Show("Invalid ROM end for bank 0x" + bank + " (" + ROMStart.ToString("X") + " - " + ROMEnd.ToString("X") + ").");
                return;
            }
            banks[bank] = new RAMBank(bank, ROMStart, ROMEnd, decompress);
            if (BankLoaded != null) BankLoaded(bank);
        }

        public void SaveBank(int bank)
        {
            RAMBank b = banks[bank];
            if (b == null || b.compressed || b.value.Length > b.ROMEnd - b.ROMStart)
            {
                MessageBox.Show("RAM bank 0x" + bank.ToString("X2") + " cannot be saved");
                return;
            }
            Array.Copy(banks[bank].value, 0, ROM, banks[bank].ROMStart, banks[bank].value.Length);
        }
    }
}
