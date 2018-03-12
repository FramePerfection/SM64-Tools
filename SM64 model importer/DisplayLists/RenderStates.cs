using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;

namespace SM64ModelImporter
{
    public class RenderStates
    {
        public enum Parameter
        {
            EnvironmentColor = 0,
            FogColor = 1,
            FogIntensity = 2,
        }

        public static int MaxParameter { get; private set; }

        public delegate DisplayList.Command ParameterSource(Parameter param);
        public ParameterSource[] getParameter = new ParameterSource[3];

        #region Blend Modes
        public enum AlphaCompare
        {
            None = 0,
            BlendColor = 1,
            Random = 3
        }
        public enum CoverageDestination
        {
            Clamp = 0,
            Wrap = 1,
            Full = 2,
            Save = 3
        }
        public enum ZMode
        {
            Opaque = 0,
            Interpenetrating = 1,
            Translucent = 2,
            Decal = 3
        }

        public int otherModesLow = 0x2078;
        public int blendMode = 0xC811;

        public AlphaCompare alphaCompare
        {
            get { return (AlphaCompare)Utility.GetBits(otherModesLow, 2, 0); }
            set { Utility.SetBits(ref otherModesLow, 2, 0, (int)value); }
        }
        public bool zSrcSelect
        {
            get { return Utility.GetBit(otherModesLow, 2); }
            set { Utility.SetBit(ref otherModesLow, 2, value); }
        }
        public bool antiAliasing
        {
            get { return Utility.GetBit(otherModesLow, 3); }
            set { Utility.SetBit(ref otherModesLow, 3, value); }
        }
        public bool zCompare
        {
            get { return Utility.GetBit(otherModesLow, 4); }
            set { Utility.SetBit(ref otherModesLow, 4, value); }
        }
        public bool zUpdate
        {
            get { return Utility.GetBit(otherModesLow, 5); }
            set { Utility.SetBit(ref otherModesLow, 5, value); }
        }
        public bool accessCoverage
        {
            get { return Utility.GetBit(otherModesLow, 6); }
            set { Utility.SetBit(ref otherModesLow, 6, value); }
        }
        public bool clearOnCoverage
        {
            get { return Utility.GetBit(otherModesLow, 7); }
            set { Utility.SetBit(ref otherModesLow, 7, value); }
        }
        public CoverageDestination coverageDest
        {
            get { return (CoverageDestination)Utility.GetBits(otherModesLow, 2, 8); }
            set { Utility.SetBits(ref otherModesLow, 2, 8, (int)value); }
        }
        public ZMode zMode
        {
            get { return (ZMode)Utility.GetBits(otherModesLow, 2, 10); }
            set { Utility.SetBits(ref otherModesLow, 2, 10, (int)value); }
        }
        public bool coverageXAlpha
        {
            get { return Utility.GetBit(otherModesLow, 12); }
            set { Utility.SetBit(ref otherModesLow, 12, value); }
        }
        public bool alphaCoverageSelect
        {
            get { return Utility.GetBit(otherModesLow, 13); }
            set { Utility.SetBit(ref otherModesLow, 13, value); }
        }
        public bool forceBlending
        {
            get { return Utility.GetBit(otherModesLow, 14); }
            set { Utility.SetBit(ref otherModesLow, 14, value); }
        }
        #endregion

        #region B7 flags
        public int RCPSet = 0x0;
        public int RCPUnset = 0x0;
        public enum RCP_OP
        {
            ignore = 0,
            set = 1,
            unset = 2
        }
        RCP_OP GetRCP_OP(int bit) { return (RCP_OP)((Utility.GetBit(RCPSet, bit)  ? 1 : 0) | (Utility.GetBit(RCPUnset, bit) ? 2 : 0)); }
        void SetRCP_OP(int bit, RCP_OP value)
        {
            Utility.SetBit(ref RCPSet, bit, value == RCP_OP.set);
            Utility.SetBit(ref RCPUnset, bit, value == RCP_OP.unset);
        }

        public RCP_OP RCP_SetShader
        {
            get { return GetRCP_OP(2); }
            set { SetRCP_OP(2, value); }
        }

        public RCP_OP RCP_FlatVtxRGBA
        {
            get { return GetRCP_OP(9); }
            set { SetRCP_OP(9, value); }
        }

        public RCP_OP RCP_CullFront
        {
            get { return GetRCP_OP(12); }
            set { SetRCP_OP(12, value); }
        }

        public RCP_OP RCP_CullBack
        {
            get { return GetRCP_OP(13); }
            set { SetRCP_OP(13, value); }
        }

        public RCP_OP RCP_Fog
        {
            get { return GetRCP_OP(16); }
            set { SetRCP_OP(16, value); }
        }

        public RCP_OP RCP_Lighting
        {
            get { return GetRCP_OP(17); }
            set { SetRCP_OP(17, value); }
        }

        public RCP_OP RCP_TexGen
        {
            get { return GetRCP_OP(18); }
            set { SetRCP_OP(18, value); }
        }

        public RCP_OP RCP_TexGenLinear
        {
            get { return GetRCP_OP(19); }
            set { SetRCP_OP(19, value); }
        }
        #endregion

        #region Combiner
        public CombinerStates combiner = new CombinerStates();
        public double textureScaleX = 1, textureScaleY = 1;
        #endregion

        public BlenderControl.CycleModes cycleType = BlenderControl.CycleModes.TwoCycle;

        static RenderStates()
        {
            MaxParameter = Enum.GetValues(typeof(Parameter)).Length;
        }

        public static DisplayList.Command CreateParameterCommand(Parameter param, int value)
        {
            switch (param)
            {
                case RenderStates.Parameter.EnvironmentColor:
                    return new DisplayList.Command(0xFB, 0, value);
                case RenderStates.Parameter.FogColor:
                    return new DisplayList.Command(0xF8, 0, value);
                case RenderStates.Parameter.FogIntensity:
                    return new DisplayList.Command(0xBC, 8, value);
                default:
                    throw new Exception("Invalid Parameter " + param.ToString());
            }
        }

        public void AppendCommands(List<DisplayList.Command> targetList, int layer)
        {
            unchecked
            {
                targetList.Add(new DisplayList.Command(0xE7, 0, 0));

                if (cycleType == BlenderControl.CycleModes.TwoCycle)
                    targetList.Add(new DisplayList.Command(0xBA, 0x1402, 0x00100000)); //Set cycle type to two cycle mode
                int blender = ((blendMode & 0xFFFF) << 0x10) | (otherModesLow & 0xFFFF);
                targetList.Add(new DisplayList.Command(0xB9, 0x31D, blender)); //Set blender
                targetList.Add(new DisplayList.Command(0xB9, 0x201, 0x0)); //Disable ZSelect (lol)

                SetParameter(targetList, Parameter.EnvironmentColor);
                SetParameter(targetList, Parameter.FogColor);
                SetParameter(targetList, Parameter.FogIntensity);

                targetList.Add(new DisplayList.Command(0xB6, 0x0, RCPUnset & 0x7FFFFFFE)); //Unset RCP bits
                targetList.Add(new DisplayList.Command(0xB7, 0x0, RCPSet & 0x7FFFFFFE)); //Set RCP bits

                targetList.Add(new DisplayList.Command(0xFC, (int)(combiner.state >> 0x20) & 0xFFFFFF, (int)(combiner.state & 0xFFFFFFFF))); //Set combiner

                targetList.Add(Commands.G_SETTILE(TextureImage.TextureFormat.G_IM_FMT_RGBA, TextureImage.BitsPerPixel.G_IM_SIZ_16b, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0));
                targetList.Add(new DisplayList.Command(0xBB, 0x1, (Math.Min(((int)(textureScaleX * (1 << 16))), 0xFFFF) << 0x10) | Math.Min((int)(textureScaleY * (1 << 16)), 0xFFFF)));
            }
        }

        void SetParameter(List<DisplayList.Command> targetList, Parameter param)
        {
            int a = (int)param;
            if (getParameter[a] == null)
                return;
            targetList.Add(getParameter[a](param));
        }

        public void Reset(List<DisplayList.Command> targetList, int layer)
        {
            if (cycleType != BlenderControl.CycleModes.OneCycle)
                targetList.Add(new DisplayList.Command(0xBA, 0x1402, 0x00000000));
            targetList.Add(new DisplayList.Command(0xB6, 0x0, RCPSet & 0x7FFFFFFE)); //Reset specifically set RCP bits
            targetList.Add(new DisplayList.Command(0xB7, 0x0, RCPUnset & 0x7FFFFFFE)); //Reset specifically Unset RCP bits
            foreach (DisplayList.Command cmd in DisplayList.defaultCommands[layer])
                targetList.Add(cmd);
        }
    }
}
