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
    public partial class ToolBox : Form
    {
        public static ToolBox instance;
        Main main;
        int[] levelAddresses = new int[0];
        public ToolBox(Main main)
        {
            instance = this;
            InitializeComponent();
            this.main = main;
            warpControl.WarpsChanged += () => {if (Editor.currentArea != null) {
                Editor.currentArea.warps.Clear(); Editor.currentArea.warps.AddRange(warpControl.GetWarps());
            }};
        }

        public void SetLevelAddresses(int[] addresses)
        {
            this.levelAddresses = addresses;
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
