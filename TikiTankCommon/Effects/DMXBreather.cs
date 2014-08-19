using System;
using System.Drawing;
namespace TikiTankCommon.Effects
{
    public class DMXBreather : IEffect
    {
        public DMXBreather()
        {            
            this.Argument = "0";
            this.Color = Color.Black;
            this.increase = true;
            this._coef = 0;
            this.last = DateTime.Now;
        }
        public void Activate(Color[] pixels)
        {
            strip = new Color[pixels.Length];
            StripHelper.FillColor(strip, 0, strip.Length, Color.Gold);
            Array.ConstrainedCopy(pixels, 0, strip, 0, pixels.Length);                        
        }
        public void Deactivate(Color[] pixels)
        {
        }
        public bool WouldUpdate()
        {
          /*  if (increase)
            {
                TimeSpan since = DateTime.Now - last;
                return (since > TimeSpan.FromMilliseconds(35));
            }
            else*/
                return true;
        }

        public void FrameUpdate(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = strip[i].MakeDarker((float)Math.Sin((double)_coef));
            }

            _coef = (increase) ? _coef + 0.04F : _coef - 0.04F;

            if (_coef > 1.2)
                increase = false;
            else if (_coef < 0.2)
                increase = true;

            this.last = DateTime.Now;
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
        float _coef;
        bool increase;
        private Color[] strip = new Color[1];
        DateTime last;        
    }
}