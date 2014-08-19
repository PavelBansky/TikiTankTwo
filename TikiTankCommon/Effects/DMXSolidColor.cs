using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class DMXSolidColor : IEffect
    {
        public DMXSolidColor()
        {
            this.Color = Color.Black;
            _arg = 0;
            startTime = DateTime.Now;
        }

        public void Activate(Color[] pixels) 
        {
            strip = new Color[pixels.Length];
            Array.Copy(pixels, 0, strip, 0, pixels.Length);           
        }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
            TimeSpan delta = DateTime.Now - startTime;
            if (delta.TotalMilliseconds > 1000)
            {
                startTime = DateTime.Now;
                return true;
            }

            return false;
        }

        public void FrameUpdate(Color[] pixels)
        {
            Array.Copy(strip, 0, pixels, 0, strip.Length);
            //return 2000;
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

        private Color[] strip = new Color[1];

        private int _arg;
        private DateTime startTime;
    }
}
