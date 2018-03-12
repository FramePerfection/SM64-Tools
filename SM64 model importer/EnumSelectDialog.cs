using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SM64ModelImporter
{
    public partial class EnumSelectDialog<T> : Form where T : struct, IConvertible
    {
        public T selectedValue { get; private set; }
        RadioButton[] btns;
        public EnumSelectDialog(params T[] exclusions)
        {
            InitializeComponent();
            string[] names = Enum.GetNames(typeof(T));
            List<string> allowedNames = new List<string>();
            string[] exclusionStrings = Array.ConvertAll<T, string>(exclusions, (t) => t.ToString());
            foreach (string name in names)
                if (Array.IndexOf(exclusionStrings, name) == -1)
                    allowedNames.Add(name);
            names = allowedNames.ToArray();
            btns = new RadioButton[names.Length];
            const int RADIO_BUTTON_HEIGHT = 20;
            Height = names.Length * RADIO_BUTTON_HEIGHT + 100;
            SuspendLayout();
            for (int i = 0; i < names.Length; i++)
            {
                RadioButton rd = new RadioButton();
                rd.Text = names[i];
                rd.Left = Left + DefaultMargin.Left;
                rd.Top = Top + DefaultMargin.Top + i * RADIO_BUTTON_HEIGHT;
                rd.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                rd.Width = ClientSize.Width;
                Controls.Add(btns[i] = rd);
            }
            if (btns.Length > 0)
                btns[0].Checked = true;
            ResumeLayout();
        }

        public void Select(T value)
        {
            string valueName = value.ToString();
            for (int i = 0; i < btns.Length; i++)
                if (btns[i].Text == valueName)
                    btns[i].Checked = true;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            selectedValue = default(T);
            for (int i = 0; i < btns.Length; i++)
                if (btns[i].Checked) selectedValue = (T)Enum.Parse(typeof(T), btns[i].Text);
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
