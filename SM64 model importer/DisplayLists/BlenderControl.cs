using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64_model_importer
{
    public partial class BlenderControl : UserControl
    {
        public event EventHandler BlendModeChanged;

        public enum PMSource
        {
            InputColor = 0,
            Framebuffer = 1,
            BlendColor = 2,
            FogColor = 3
        }
        public enum ASource
        {
            InputAlpha = 0,
            FogAlpha = 1,
            ShadeAlpha = 2,
            Zero = 3
        }
        public enum BSource
        {
            InvInputAlpha = 0,
            FramebufferAlpha = 1,
            One = 2,
            Zero = 3
        }
        public enum CycleModes
        {
            OneCycle = 0,
            TwoCycle = 1,
            CopyTexture = 2,
            Fill = 3
        }

        public BlenderControl()
        {
            InitializeComponent();
            cmbCycleMode.DataSource = Enum.GetValues(typeof(CycleModes));
            cmbP1.DataSource = Enum.GetValues(typeof(PMSource));
            cmbP2.DataSource = Enum.GetValues(typeof(PMSource));
            cmbM1.DataSource = Enum.GetValues(typeof(PMSource));
            cmbM2.DataSource = Enum.GetValues(typeof(PMSource));
            cmbA1.DataSource = Enum.GetValues(typeof(ASource));
            cmbA2.DataSource = Enum.GetValues(typeof(ASource));
            cmbB1.DataSource = Enum.GetValues(typeof(BSource));
            cmbB2.DataSource = Enum.GetValues(typeof(BSource));

            cmbCycleMode.SelectedItem = CycleModes.TwoCycle;
            initialized = true;
            SetValues(0xC811);
        }

        bool initialized = false;

        private void cmbCycleMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbP2.Enabled = cmbA2.Enabled = cmbM2.Enabled = cmbB2.Enabled = ((CycleModes)cmbCycleMode.SelectedItem == CycleModes.TwoCycle);
        }

        private void blendModeChanged(object sender, EventArgs e)
        {
            if (BlendModeChanged != null && initialized)
                BlendModeChanged(this, e);
        }

        public void SetValues(int value)
        {
            cmbP1.SelectedItem = (PMSource)((value >> 0xE) & 0x3);
            cmbP2.SelectedItem = (PMSource)((value >> 0xC) & 0x3);
            cmbA1.SelectedItem = (ASource)((value >> 0xA) & 0x3);
            cmbA2.SelectedItem = (ASource)((value >> 0x8) & 0x3);
            cmbM1.SelectedItem = (PMSource)((value >> 0x6) & 0x3);
            cmbM2.SelectedItem = (PMSource)((value >> 0x4) & 0x3);
            cmbB1.SelectedItem = (BSource)((value >> 0x2) & 0x3);
            cmbB2.SelectedItem = (BSource)((value >> 0x0) & 0x3);
        }

        public int GetValues()
        {
            return ((int)cmbP1.SelectedItem << 0xE) |
                 ((int)cmbP2.SelectedItem << 0xC) |
                 ((int)cmbA1.SelectedItem << 0xA) |
                 ((int)cmbA2.SelectedItem << 0x8) |
                 ((int)cmbM1.SelectedItem << 0x6) |
                 ((int)cmbM2.SelectedItem << 0x4) |
                 ((int)cmbB1.SelectedItem << 0x2) |
                 ((int)cmbB2.SelectedItem << 0x0);
        }

    }
}
