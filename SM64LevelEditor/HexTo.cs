using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace SM64LevelEditor
{
    public class HexTo<T> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ? true : base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) ? true : base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(T))
                return string.Format("0x{0:X" + (System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)) * 2) + "}", value);
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = (string)value;
                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    input = input.Substring(2);
                MethodInfo meth = typeof(T).GetMethod("Parse", new Type[] { typeof(string), typeof(System.Globalization.NumberStyles), typeof(IFormatProvider) });
                if (meth == null || !meth.IsStatic)
                    return base.ConvertFrom(context, culture, value);
                try
                {
                    return meth.Invoke(null, new object[] { input, System.Globalization.NumberStyles.HexNumber, culture });
                }
                catch
                {
                    return null;
                }
            }
            else
                return base.ConvertFrom(context, culture, value);
        }
    }
}
