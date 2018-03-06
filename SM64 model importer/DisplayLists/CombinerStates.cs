using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;

namespace SM64_model_importer
{
    public class CombinerStates
    {
        public long state = 0x127FFFFFFFF838;

        #region Enums
        public enum ColorA
        {
            Combined = 0, Texel0 = 1, Texel1 = 2, Primitive = 3, Shade = 4, Environment = 5,
            One = 6,
            Noise = 7,
            Zero = 0xF
        }

        public enum ColorB
        {
            Combined = 0, Texel0 = 1, Texel1 = 2, Primitive = 3, Shade = 4, Environment = 5,
            Center = 6,
            K4 = 7,
            Zero = 0xF
        }

        public enum ColorC
        {
            Combined = 0, Texel0 = 1, Texel1 = 2, Primitive = 3, Shade = 4, Environment = 5,
            Scale = 6,
            Combined_Alpha = 7,
            Texel0_Alpha = 8,
            Texel1_Alpha = 9,
            Primitive_Alpha = 0xA,
            Shade_Alpha = 0xB,
            Environment_Alpha = 0xC,
            Primitive_LOD_FRAC = 0xE,
            K5 = 0xF,
            Zero = 0x1F
        }

        public enum ColorD
        {
            Combined = 0, Texel0 = 1, Texel1 = 2, Primitive = 3, Shade = 4, Environment = 5,
            One = 6,
            Zero = 7,
        }


        public enum AlphaABD
        {
            Combined = 0,
            Texel0 = 1, Texel1 = 2, Primitve = 3, Shade = 4, Environment = 5,
            One,
            Zero
        }

        public enum AlphaC
        {
            LOD_FRAC = 0,
            Texel0 = 1, Texel1 = 2, Primitve = 3, Shade = 4, Environment = 5,
            Primitive_LOD_FRAC,
            Zero
        }

        #endregion

        #region Properties

        public ColorA a0
        {
            get { return (ColorA)Utility.GetBits(state, 4, 0x34); }
            set { Utility.SetBits(ref state, 4, 0x34, (int)value); }
        }

        public ColorC c0
        {
            get { return (ColorC)Utility.GetBits(state, 5, 0x2F); }
            set { Utility.SetBits(ref state, 5, 0x2F, (int)value); }
        }

        public AlphaABD Aa0
        {
            get { return (AlphaABD)Utility.GetBits(state, 3, 0x2C); }
            set { Utility.SetBits(ref state, 3, 0x2C, (int)value); }
        }

        public AlphaC Ac0
        {
            get { return (AlphaC)Utility.GetBits(state, 5, 0x29); }
            set { Utility.SetBits(ref state, 5, 0x29, (int)value); }
        }

        public ColorA a1
        {
            get { return (ColorA)Utility.GetBits(state, 5, 0x25); }
            set { Utility.SetBits(ref state, 5, 0x25, (int)value); }
        }

        public ColorC c1
        {
            get { return (ColorC)Utility.GetBits(state, 5, 0x20); }
            set { Utility.SetBits(ref state, 5, 0x20, (int)value); }
        }

        public ColorB b0
        {
            get { return (ColorB)Utility.GetBits(state, 4, 0x1C); }
            set { Utility.SetBits(ref state, 4, 0x1C, (int)value); }
        }

        public ColorB b1
        {
            get { return (ColorB)Utility.GetBits(state, 4, 0x18); }
            set { Utility.SetBits(ref state, 4, 0x18, (int)value); }
        }

        public AlphaABD Aa1
        {
            get { return (AlphaABD)Utility.GetBits(state, 3, 0x15); }
            set { Utility.SetBits(ref state, 3, 0x15, (int)value); }
        }

        public AlphaC Ac1
        {
            get { return (AlphaC)Utility.GetBits(state, 3, 0x12); }
            set { Utility.SetBits(ref state, 3, 0x12, (int)value); }
        }

        public ColorD d0
        {
            get { return (ColorD)Utility.GetBits(state, 3, 0xF); }
            set { Utility.SetBits(ref state, 3, 0xF, (int)value); }
        }

        public AlphaABD Ab0
        {
            get { return (AlphaABD)Utility.GetBits(state, 3, 0xC); }
            set { Utility.SetBits(ref state, 3, 0xC, (int)value); }
        }

        public AlphaABD Ad0
        {
            get { return (AlphaABD)Utility.GetBits(state, 3, 0x9); }
            set { Utility.SetBits(ref state, 3, 0x9, (int)value); }
        }

        public ColorD d1
        {
            get { return (ColorD)Utility.GetBits(state, 3, 0x6); }
            set { Utility.SetBits(ref state, 3, 0x6, (int)value); }
        }

        public AlphaABD Ab1
        {
            get { return (AlphaABD)Utility.GetBits(state, 3, 0x3); }
            set { Utility.SetBits(ref state, 3, 0x3, (int)value); }
        }

        public AlphaABD Ad1
        {
            get { return (AlphaABD)Utility.GetBits(state, 3, 0x0); }
            set { Utility.SetBits(ref state, 3, 0x0, (int)value); }
        }

        #endregion
    }
}
