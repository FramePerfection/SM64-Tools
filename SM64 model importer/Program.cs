using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace SM64ModelImporter
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            byte[] arr = new byte[100];
            Application.Run(new Main());
        }
    }
}
