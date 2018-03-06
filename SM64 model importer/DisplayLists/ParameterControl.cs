using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SM64RAM;

namespace SM64_model_importer
{
    [DefaultEvent("TypeChanged")]
    public partial class ParameterControl : UserControl
    {
        public delegate void Action();
        public event Action ValueChanged, TypeChanged;
        int val = 0;
        public int customValue { get { return val; } set { txtValue.Text = (val = value).ToString("X8"); } }

        public string ParameterName
        {
            get { return groupParameter.Text; }
            set { groupParameter.Text = value;}
        }


        public ParameterControl()
        {
            InitializeComponent();
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            int newVal;
            if (cvt.ParseIntHex(txtValue.Text, out newVal))
                val = newVal;
            if (ValueChanged != null) ValueChanged();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValue.Enabled = cmbType.SelectedIndex == cmbType.Items.Count - 1;
            if (TypeChanged != null) TypeChanged();
        }
    }
}
