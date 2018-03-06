using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SM64Patcher
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files|*.txt";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            if (MessageBox.Show("Are you sure you want to apply these patches?", "Confirm", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes) return;
            string txt = File.ReadAllText(ofd.FileName);
            txt = System.Text.RegularExpressions.Regex.Replace(txt, "\\s", " ");
            SM64RAM.PatchEngine.Run(txt, Path.GetDirectoryName(ofd.FileName));
        }

        private void btnLoadROM_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SM64 ROMs (*.z64)|*.z64";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            SM64RAM.EmulationState.instance.LoadROM(ofd.FileName);
            lblROM.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
            btnLoadFile.Enabled = true;
        }
    }
}
