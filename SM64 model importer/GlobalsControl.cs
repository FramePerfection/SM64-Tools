using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64_model_importer
{
    public partial class GlobalsControl : UserControl, Importable, ReadWrite
    {
        public bool useCustomAddress { get { return false; } }
        static DisplayList.Command cmdReturn = new DisplayList.Command(0xB8, 0, 0);

        int[] globalParameters = new int[] { (0x7FFFFFC4 << 1), 0x7F90A0FF, 0x1A00E800 };
        int[] parameterAddress = new int[RenderStates.MaxParameter];

        public GlobalsControl()
        {
            InitializeComponent();
            numFogIntensity.Validating += QuantizeNumeric;
            numFogOffset.Validating += QuantizeNumeric;
        }

        void QuantizeNumeric(object sender, EventArgs e)
        {
            NumericUpDown num = sender as NumericUpDown;
            if (num == null) return;
            num.Value = (int)(num.Value * 256 + (decimal)0.5) / (decimal)256;
            int mul = (int)(numFogIntensity.Value * 256 + (decimal)0.5);
            int off = (int)(numFogOffset.Value * 256 + (decimal)0.5);
            globalParameters[(int)RenderStates.Parameter.FogIntensity] = (mul << 0x10) | off;
        }

        public int GetParameterAddress(RenderStates.Parameter parameter)
        {
            return parameterAddress[(int)parameter];
        }

        public int PrepareForImport() { return 0; /*Nothing to prepare*/}

        public int Import(int segmentedPointer)
        {
            int bank = segmentedPointer >> 0x18;
            int cursor = segmentedPointer;
            if (segmentedPointer != Main.instance.baseOffset)
                return -1;
            for (int i = 0; i < RenderStates.MaxParameter; i++)
            {
                parameterAddress[i] = cursor;
                DisplayList.Command cmd = RenderStates.CreateParameterCommand((RenderStates.Parameter)i, globalParameters[i]);
                Array.Copy(cmd.values, 0, SM64RAM.EmulationState.instance.banks[bank].value, cursor & 0xFFFFFF, 8);
                Array.Copy(cmdReturn.values, 0, SM64RAM.EmulationState.instance.banks[bank].value, (cursor & 0xFFFFFF) + 8, 8);
                cursor += 0x10;
            }
            return cursor;
        }

        private void btnFogColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return;
            Color c = dlg.Color;
            globalParameters[(int)RenderStates.Parameter.FogColor] = (c.R << (int)0x18) | (c.G << (int)0x10) | (c.B << (int)0x08) | c.A;

        }

        public void LoadSettings(FileParser.Block block)
        {
            string[] names = Enum.GetNames(typeof(RenderStates.Parameter));
            for (int i = 0; i < names.Length; i++)
            {
                int fileValue = block.GetInt(names[i], false);
                globalParameters[i] = fileValue;
            }
            numFogIntensity.Value = (globalParameters[(int)RenderStates.Parameter.FogIntensity] >> 0x10) / (decimal)256;
            numFogOffset.Value = (globalParameters[(int)RenderStates.Parameter.FogIntensity] & 0xFFFF) / (decimal)256;
        }
        public void SaveSettings(FileParser.Block block)
        {
            string[] names = Enum.GetNames(typeof(RenderStates.Parameter));
            for (int i = 0; i < names.Length; i++)
                block.SetInt(names[i], globalParameters[i]);
        }
    }
}
