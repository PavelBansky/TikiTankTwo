using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class Rainbow : Effect
    {
        public Rainbow(EffectInfo info, LEDStrip strip)
        {
            this.Information = info;
            this.LedStrip = strip;
            this.Argument = "8";
            _counter = 0;
        }

        public override void Activate() { }

        public override void Deactivate() { }

        public override int Step()
        {
            int i;

            for (i = 0; i < LedStrip.Length; i++)
            {
                LedStrip.SetPixelColor((LedStrip.Length-1) - i, Color.Wheel(((i * 384 / 30) + _counter) % 384));
            }
            LedStrip.Show(); // write all the pixels out

            //cycles of all 384 colors in the wheel
            _counter = (_counter < 384) ? _counter + 10 : 0;

            return _delay;
        }

        public override string Argument
        {
            get
            {
                return _arg.ToString();
            }
            set
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    _arg = Math.Abs(i);
                    _delay = 400 / _arg;
                }
            }
        }

        public override Color Color
        {
            get;
            set;
        }
        
        private int _delay;
        private int _arg;
        private int _counter;
    }
}
