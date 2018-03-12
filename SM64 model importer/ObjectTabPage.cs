using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64ModelImporter
{
    public partial class ObjectTabPage : UserControl
    {
        TabPage parentPage;
        string type = "";
        public ObjectTabPage(Control ctrl, TabPage parentPage, string type)
        {
            this.parentPage = parentPage;
            this.type = type;
            InitializeComponent();
            Bounds = parentPage.Bounds;
            parentPage.Text = type;
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            int y = txtObjectName.Bottom + txtObjectName.Margin.Bottom;
            ctrl.Bounds = new Rectangle(this.Left, y, this.Width, this.Height - y);
            ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Controls.Add(ctrl);
        }

        private void txtObjectName_TextChanged(object sender, EventArgs e)
        {
            if (txtObjectName.Text.Length == 0)
                parentPage.Text = type;
            else
                parentPage.Text = type + ": " + txtObjectName.Text;
        }

        private void btnDeleteObject_Click(object sender, EventArgs e)
        {
            Main.instance.RemoveDisplayList(Controls[2] as DisplayListControl, parentPage);
            Main.instance.RemoveCollision(Controls[2] as CollisionControl, parentPage);
        }
    }
}
