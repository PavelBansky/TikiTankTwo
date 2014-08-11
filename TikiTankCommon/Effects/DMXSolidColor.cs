using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class DMXSolidColor : IEffect
    {
        public DMXSolidColor()
        {
            this.Color = Color.Black;
            _arg = 0;
        }

        public void Activate(Color[] pixels) 
        {
            strip = new Color[pixels.Length];
            Array.Copy(pixels, 0, strip, 0, pixels.Length);
           // StripHelper.FillColor(strip, 0, strip.Length, Color.Black);            
        }

        public void Deactivate(Color[] pixels) { }

        public int Update(Color[] pixels)
        {
            Array.Copy(strip, 0, pixels, 0, strip.Length);
            return 2000;
        }

        public string Argument
        {
            get
            {
                return _arg.ToString();
            }
            set 
            {
                int a;
                if (int.TryParse(value, out a))
                {
                    if (a >= 0 && a < strip.Length)
                    {
                        _arg = a;
                    }
                }
            }
        }

        public Color Color
        {
            get
            {
                return strip[_arg];
            }
            set
            {
               strip[_arg] = value;
            }
        }

        private Color[] strip = new Color[1];

        private int _arg;
    }
}
