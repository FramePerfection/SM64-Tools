using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SM64LevelEditor
{
    public partial class LogWindow : Form, SM64RAM.MessageStream
    {
        public LogWindow()
        {
            InitializeComponent();
            FormClosing += (_, e) => { e.Cancel = true; Hide(); }; //Hide this form as to not dispose it.
        }
        public void AppendMessage(string message, string type)
        {
            txtLog.AppendText(type + ": " + message + "\n");
        }
    }
}
