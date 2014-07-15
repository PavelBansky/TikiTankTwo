using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class DMXSolidColor : Effect
    {
        public DMXSolidColor(EffectInfo info, LEDStrip strip)
        {
            this.Information = info;
            this.LedStrip = strip;
            _arg = 0;
        }

        public override void Activate() { }

        public override void Deactivate() { }

        public override int Step()
        {
            LedStrip.Show();

            return 2000;
        }

        public override string Argument
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
                    if (a >= 0 && a < LedStrip.Length)
                    {
                        _arg = a;
                    }
                }
            }
        }

        public override Color Color
        {
            get
            {
                return Color.FromRgb(LedStrip.GetPixelColor(_arg));
            }
            set
            {
                LedStrip.SetPixelColor(_arg, value);
            }
        }

        private int _arg;
    }
}
