using System;

namespace GroundZero.Localization
{
    public class PluralFormatProvider : IFormatProvider, ICustomFormatter
    {
        private static PluralFormatProvider mFormatter;
        public static PluralFormatProvider Formatter
        {
            get
            {
                if(mFormatter == null)
                {
                    mFormatter = new PluralFormatProvider();
                }
                return mFormatter;
            }
        }

        public object GetFormat(Type formatType)
        {
            return this;
        }


        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (arg == null)
            {
                return string.Empty;
            }
            if (format == null)
            {
                if (arg is IFormattable)
                {
                    return ((IFormattable)arg).ToString(format, provider);
                }
                return arg.ToString();
            }
            else
            {
                string[] forms = format.Split(';');
                if(forms.Length == 2)
                {
                    int value = (int)arg;
                    int form = (value == 1 || value == 0) ? 0 : 1;
                    return value.ToString() + " " + forms[form];
                }
                else
                {
                    if (arg is IFormattable)
                    {
                        return ((IFormattable)arg).ToString(format, provider);
                    }
                    return arg.ToString();
                }
            }


          

        }
      

    }
}