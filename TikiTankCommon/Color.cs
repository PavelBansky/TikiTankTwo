using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TikiTankCommon
{
    public class Color
    {
        public static Color FromRgb(byte red, byte green, byte blue)
        {
            Color clr = new Color();
            clr.R = red;
            clr.G = green;
            clr.B = blue;

            return clr;
        }

        public static Color FromRgb(int color)
        {
            Color clr = new Color();
            clr.R = (byte)(color >> 16);
            clr.G = (byte)(color >> 8);
            clr.B = (byte)color;

            return clr;
        }

        public int ToRgb()
        {
            int i = this.B;
            i <<= 8;
            i |= this.G;
            i <<= 8;
            i |= this.R;

            return i;
        }


        // Returns Color parsed from color string 
        public static Color ColorStringToColor(string kobcol)
        {            
            return (Color.FromRgb(int.Parse(kobcol, NumberStyles.AllowHexSpecifier)));
        }

        // Convert color to color string
        public static string ColorToColorString(Color col)
        {            
            return col.ToRgb().ToString("X2");
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}
