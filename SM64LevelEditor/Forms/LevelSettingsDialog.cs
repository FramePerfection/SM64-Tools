using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace SM64LevelEditor
{
    public partial class LevelSettingsDialog : Form
    {
        class LevelBankDescription
        {
            public string[] objects = new string[0];
            public Dictionary<string, byte[][]> commandBytes = new Dictionary<string, byte[][]>();
        }
        static Dictionary<string, Dictionary<string, LevelBankDescription>> bankOptions;
        static LevelSettingsDialog()
        {
            ParseFiles("Object Bank Data");
        }

        static void ParseFiles(string directory)
        {
            bankOptions = new Dictionary<string, Dictionary<string, LevelBankDescription>>();
            foreach (string file in System.IO.Directory.GetFiles(directory, "Bank 0x*.txt"))
                bankOptions[file.Remove(0, directory.Length + 1)] = ParseFile(file);
        }

        static Dictionary<string, LevelBankDescription> ParseFile(string file)
        {
            StreamReader reader = new StreamReader(file);
            try
            {
                LevelBankDescription currentDescription = null;
                Dictionary<string, LevelBankDescription> output = new Dictionary<string, LevelBankDescription>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Split(';')[0].Trim();
                    if (line.Length == 0) continue;
                    if (line[0] == '[' && line[line.Length - 1] == ']') //option beginning
                        output[line.Substring(1, line.Length - 2)] = currentDescription = new LevelBankDescription();
                    else
                    {
                        string[] split = line.Split('=');
                        string cmd = split[0].Trim().ToLower();
                        if (cmd == "list") //List command
                            currentDescription.objects = split[1].Trim().Split('|');
                        else
                            currentDescription.commandBytes[cmd] = Array.ConvertAll(split[1].Trim().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries),
                                (s) => Array.ConvertAll(s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), (b) => byte.Parse(b.Trim(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture)));
                    }
                }
                return output;
            }
            finally
            {
                reader.Close();
            }
        }

        LevelBankDescription bank0x5, bank0x6, bank0x9;

        public LevelSettingsDialog()
        {
            InitializeComponent();
            foreach (string str in bankOptions["Bank 0x9.txt"].Keys)
                cmbBank0x9_0x12.Items.Add(str);
            foreach (string str in bankOptions["Bank 0xC.txt"].Keys)
                cmbBank0x5_0xC.Items.Add(str);
            foreach (string str in bankOptions["Bank 0xD.txt"].Keys)
                cmbBank0x6_0xD.Items.Add(str);
        }

        private void cmbBank0x9_0x12_SelectedIndexChanged(object sender, EventArgs e)
        {

            lstBankObjects0x9_0x12.DataSource = null;
            bank0x9 = bankOptions["Bank 0x9.txt"][cmbBank0x9_0x12.Text];
            lstBankObjects0x9_0x12.DataSource = bank0x9.objects;
        }

        private void cmbBank0x5_0xC_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstBankObjects0x5_0xC.DataSource = null;
            bank0x5 = bankOptions["Bank 0xC.txt"][cmbBank0x5_0xC.Text];
            lstBankObjects0x5_0xC.DataSource = bank0x5.objects;
        }

        private void cmbBank0x6_0xD_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstBankObjects0x6_0xD.DataSource = null;
            bank0x6 = bankOptions["Bank 0xD.txt"][cmbBank0x6_0xD.Text];
            lstBankObjects0x6_0xD.DataSource = bank0x6.objects;
        }

        void WriteLevelData()
        {
            foreach (LevelBankDescription bank in new LevelBankDescription[] { bank0x5, bank0x6, bank0x9 })
            {
                if (bank == null) continue;
                byte[][] cmd17;
                if (bank.commandBytes.TryGetValue("cmd17", out cmd17))
                    foreach (byte[] cmd in cmd17)
                    {
                        BankDescription newDescription = BankDescription.Parse(cmd);
                        foreach (BankDescription desc in new List<BankDescription>(Editor.currentLevel.loadedBanks))
                            if (desc.ID == newDescription.ID)
                                Editor.currentLevel.loadedBanks.Remove(desc);
                        Editor.currentLevel.loadedBanks.Add(newDescription);
                        SM64RAM.EmulationState.instance.banks[newDescription.ID] = new SM64RAM.EmulationState.RAMBank(newDescription.ID, (int)newDescription.ROM_Start, (int)newDescription.ROM_End, cmd[0] != 0x17);
                    }
                byte[][] cmd1A;
                if (bank.commandBytes.TryGetValue("cmd1a", out cmd1A))
                    foreach (byte[] cmd in cmd1A)
                    {
                        BankDescription newDescription = BankDescription.Parse(cmd);
                        foreach (BankDescription desc in new List<BankDescription>(Editor.currentLevel.loadedBanks))
                            if (desc.ID == newDescription.ID)
                                Editor.currentLevel.loadedBanks.Remove(desc);
                        Editor.currentLevel.loadedBanks.Add(newDescription);
                        SM64RAM.EmulationState.instance.banks[newDescription.ID] = new SM64RAM.EmulationState.RAMBank(newDescription.ID, (int)newDescription.ROM_Start, (int)newDescription.ROM_End, true);
                    }
                byte[][] cmd22;
                if (bank.commandBytes.TryGetValue("cmd22", out cmd22))
                    foreach (byte[] cmd in cmd22)
                        Editor.currentLevel.AddLevelGeo(cmd[3], Level.modelIDs[cmd[3]] = GeoLayout.LoadSegmented(SM64RAM.cvt.int32(cmd, 4)));

                byte[][] cmd06;
                if (bank.commandBytes.TryGetValue("cmd06", out cmd06))
                    foreach (byte[] cmd in bank.commandBytes["cmd06"])
                        Editor.currentLevel.ReadJump(cmd);
                Editor.currentLevel.CleanLevelGeos();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            WriteLevelData();
        }
    }
}
