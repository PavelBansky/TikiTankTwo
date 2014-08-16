using System.Drawing;
using System.Globalization;

namespace TikiTankCommon
{
    public static class ColorHelper
    {
        public static Color StringToColor(string colorString)
        {
            Color result;
            if (colorString.StartsWith("#"))
            {                
                result = ColorHelper.FromRgb(int.Parse(colorString.Substring(1), NumberStyles.AllowHexSpecifier));
            }
            else
            {                
                result = Color.FromName(colorString);
            }

            return result;
        }

        public static string ColorToString(Color col)
        {
            string retstr = col.ToArgb().ToString("X2");
            if (!string.IsNullOrEmpty(retstr))
            {
                retstr = "#" + retstr.Remove(0, 2);
            }

            return retstr;
        }

        public static Color FromRgb(int color)
        {
            Color clr = Color.FromArgb((byte)(color >> 16), (byte)(color >> 8), (byte)color);

            return clr;
        }

        public static int ToRgb(Color clr)
        {
            int i = clr.B;
            i <<= 8;
            i |= clr.G;
            i <<= 8;
            i |= clr.R;

            return i;
        }

        public static Color Wheel(int WheelPos)
        {
            int r = 0, g = 0, b = 0;
            switch (WheelPos / 128)
            {
                case 0:
                    r = 127 - WheelPos % 128; // red down
                    g = WheelPos % 128; // green up
                    b = 0; // blue off
                    break;
                case 1:
                    g = 127 - WheelPos % 128; // green down
                    b = WheelPos % 128; // blue up
                    r = 0; // red off
                    break;
                case 2:
                    b = 127 - WheelPos % 128; // blue down
                    r = WheelPos % 128; // red up
                    g = 0; // green off
                    break;
            }

            return Color.FromArgb((byte)r, (byte)g, (byte)b);
        }
    }
}
