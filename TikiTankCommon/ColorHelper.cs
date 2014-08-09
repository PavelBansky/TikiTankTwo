using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

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

        public static Color FromRgb(int color)
        {
            Color clr = Color.FromArgb((color >> 16), (color >> 8), (byte)color);
 
            return clr;
        }
    }
}
