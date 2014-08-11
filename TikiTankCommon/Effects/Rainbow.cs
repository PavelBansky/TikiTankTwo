using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class Rainbow : IEffect
    {
        public Rainbow()
        {
            this.Argument = "8";
            _counter = 0;
        }

        public void Activate(Color[] pixels) { }

        public void Deactivate(Color[] pixels) { }

        public int Update(Color[] pixels)
        {
            int i;

            for (i = 0; i < pixels.Length; i++)
            {
                pixels[(pixels.Length-1) - i] = ColorHelper.Wheel(((i * 384 / 30) + _counter) % 384);
            }
            //LedStrip.Show(); // write all the pixels out

            //cycles of all 384 colors in the wheel
            _counter = (_counter < 384) ? _counter + 10 : 0;

            return _delay;
        }

        public string Argument
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

        public Color Color
        {
            get;
            set;
        }
        
        private int _delay;
        private int _arg;
        private int _counter;

    }
}
