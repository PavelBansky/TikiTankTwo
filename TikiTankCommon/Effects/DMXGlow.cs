using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class DMXGlow : IEffect
    {
        public DMXGlow()
        {
            rng = new Random();
            this.Argument = "0";
            this.Color = Color.Black;
        }

        public void Activate(Color[] pixels)
        {
            strip = new Color[pixels.Length];
            Array.ConstrainedCopy(pixels, 0, strip, 0, pixels.Length);
            //StripHelper.FillColor(strip, 0, strip.Length, Color.Black);   
        }

        public void Deactivate(Color[] pixels)
        {
           
        }

        public bool WouldUpdate()
        {
            return true;
        }

        public void FrameUpdate(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                Color current = Color.FromArgb(
                    strip[i].R - rng.Next(0, strip[i].R / 16 + 1),
                    strip[i].G - rng.Next(0, strip[i].G / 16 + 1),
                    strip[i].B - rng.Next(0, strip[i].B / 16 + 1)
                    );

                pixels[i] = current;
            }            
        }

        public void Tick()
        {

        }

        public bool IsSensorDriven { get; set; }

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

        private int _arg;
        private Color[] strip = new Color[1];
        private Random rng;
    }
}
