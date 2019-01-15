using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SM64RAM;

namespace SM64ModelImporter
{
    public partial class CollisionTypeControl : UserControl
    {
        public static string[] alias;

        static CollisionTypeControl()
        {
            if (!File.Exists("CollisionTypes.txt"))
            {
                alias = new string[0];
                return;
            }
            StreamReader reader = new StreamReader("CollisionTypes.txt");
            List<string> lst = new List<string>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] split = line.Split(new char[] { ':' }, 2);
                if (split.Length < 2) continue;
                lst.Add(line);
            }
            alias = lst.ToArray();
        }

        CollisionPatch _patch;
        public CollisionPatch patch
        {
            get { return _patch; }
            set { _patch = value; groupType.Text = _patch.name; cmbType_SelectedTextChanged(null, null); }
        }

        public bool enableImport
        {
            get { return chkEnableImport.Checked; }
            set { chkEnableImport.Checked = value; }
        }

        public Image previewImage
        {
            get { return picMaterial.BackgroundImage; }
            set { picMaterial.BackgroundImage = value; }
        }

        public CollisionTypeControl()
        {
            InitializeComponent();
            cmbType.DataSource = alias;
        }

        public void SetType(int type)
        {
            for (int i = 0; i < alias.Length; i++)
            {
                int aliasValue;
                if (cvt.ParseIntHex(alias[i].Substring(2, 2), out aliasValue) && type == aliasValue)
                {
                    cmbType.SelectedIndex = i;
                    return;
                }
            }
            cmbType.Text = "0x" + type.ToString("X2");
        }

        private void cmbType_SelectedTextChanged(object sender, EventArgs e)
        {
            if (patch == null) return;
            int type;
            if (cmbType.Text.Length >= 4 && cvt.ParseIntHex(cmbType.Text.Substring(2, 2), out type))
                patch.type = (short)type;
        }
    }
}
