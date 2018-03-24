using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64LevelEditor.Controls
{
    public partial class WarpControl : UserControl
    {
        public delegate void Action();
        public event Action WarpsChanged;

        Warp[] warpArray = new Warp[0];
        public WarpControl()
        {
            InitializeComponent();
        }

        public Warp[] GetWarps()
        {
            return (Warp[])warpArray.Clone();
        }

        public void SetWarps(Warp[] warps)
        {
            warpArray = warps;
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < warpArray.Length; i++)
            {
                b.Append(warps[i].sourceID.ToString("X") + " => " + warps[i].destinationLevel.ToString("X") + ", " + warps[i].destinationArea.ToString("X") + ", " + warps[i].destinationID.ToString("X"));
                if (i < warpArray.Length - 1)
                    b.Append("\n");
            }
            txtWarps.Clear();
            txtWarps.Text = b.ToString();
        }

        private void txtWarps_TextChanged(object sender, EventArgs e)
        {
            List<Warp> warps = new List<Warp>();
            int i = 0;
            bool changed = false;
            foreach (string line in txtWarps.Lines)
            {
                Warp newWarp = Warp.FromString(line);
                if (newWarp != null)
                {
                    warps.Add(newWarp);
                    if (i >= warpArray.Length ||
                        warps[i].sourceID != newWarp.sourceID || warps[i].destinationLevel != newWarp.destinationID || warps[i].destinationArea != newWarp.destinationArea || warps[i].destinationID != newWarp.destinationID)
                        changed = true;
                    i++;
                }
            }
            if (changed)
            {
                warpArray = warps.ToArray();
                if (WarpsChanged != null) WarpsChanged();
            }
        }
    }
}
