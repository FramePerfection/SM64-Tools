using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SM64RAM;
using SM64Renderer;

namespace SM64LevelEditor
{
    struct LevelAddress
    {
        int value;
        LevelAddress(int value) { this.value = value; }
        public static implicit operator int(LevelAddress value) { return value.value; }
        public static implicit operator LevelAddress(int value) { return new LevelAddress(value); }
        public override string ToString()
        {
            if (Editor.projectSettings != null)
                return Editor.projectSettings.GetLevelName(value) + " (0x" + value.ToString("X") + ")";
            return "0x" + value.ToString("X");
        }
    }

    public partial class ToolBox : Form
    {
        public static ToolBox instance;
        Main main;
        LevelAddress[] levelAddresses = new LevelAddress[0];
        public ToolBox(Main main)
        {
            instance = this;
            InitializeComponent();
            this.main = main;
            warpControl.WarpsChanged += () =>
            {
                if (Editor.currentArea != null)
                {
                    Editor.currentArea.warps.Clear(); 
                    Editor.currentArea.warps.AddRange(warpControl.GetWarps());
                }
            };
            FormClosing += (_, e) => { e.Cancel = true; Hide(); };
        }

        public void SetLevelAddresses(int[] addresses)
        {
            this.levelAddresses = new LevelAddress[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
                this.levelAddresses[i] = addresses[i];
            cmbLevel.DataSource = null;
            cmbLevel.DataSource = levelAddresses;
        }

        public void UpdateAlias() { objectControl1.UpdateAlias(); }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLevel.SelectedIndex >= 0 && cmbLevel.SelectedIndex < levelAddresses.Length)
                main.LoadLevel(levelAddresses[cmbLevel.SelectedIndex]);
        }
    }
}
