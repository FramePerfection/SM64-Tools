using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter
{
    public class ConversionSettings
    {
        public enum ColorInterpretation
        {
            Undefined,
            Ignore,
            ReplaceNormal,
            ConvertRedToAlpha,
            ConvertGreenToAlpha,
            ConvertBlueToAlpha,
            ConvertValueToAlpha
        }
        public ColorInterpretation colorInterpretation;

        public bool DoColorInterpretationDialog()
        {

            EnumSelectDialog<ColorInterpretation> dlg = new EnumSelectDialog<ColorInterpretation>(ColorInterpretation.Undefined);
            dlg.Text = "Vertex Color Conversion";
            dlg.Select(colorInterpretation);
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return false;
            colorInterpretation = dlg.selectedValue;
            return true;
        }

    }
}
