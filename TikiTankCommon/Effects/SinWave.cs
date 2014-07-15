using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class SinWave : Effect
    {
        public SinWave(EffectInfo info, LEDStrip strip)
        {
            this.Information = info;
            this.LedStrip = strip;
            _counter = 0;
            this.Argument = "8";
            this.Color = Color.FromRgb(255, 0, 0);
        }

        public override void Activate() { }

        public override void Deactivate() { }

        public override int Step()
        {
            Color c2 = new Color();
            float y;


            for (int i = 0; i < LedStrip.Length; i++)
            {
                y = (float)Math.Sin(Math.PI * (float)_cycles * (float)(_counter + i) / (float)LedStrip.Length);
                if (y >= 0.0)
                {
                    // Peaks of sine wave are white
                    y = 1.0F - y; // Translate Y to 0.0 (top) to 1.0 (center)
                    c2.R = (byte)(127 - (byte)((float)(127 - _color.R) * y));
                    c2.G = (byte)(127 - (byte)((float)(127 - _color.G) * y));
                    c2.B = (byte)(127 - (byte)((float)(127 - _color.B) * y));
                }
                else
                {
                    // Troughs of sine wave are black
                    y += 1.0F; // Translate Y to 0.0 (bottom) to 1.0 (center)
                    c2.R = (byte)((float)_color.R * y);
                    c2.G = (byte)((float)_color.G * y);
                    c2.B = (byte)((float)_color.B * y);
                }

                LedStrip.SetPixelColor(i, c2);
            }

            LedStrip.Show();

            _counter++;
           
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
            get
            {
                return this._color;
            }
            set
            {
                _color = value;
            }
        }

        private Color _color;
        private UInt16 _counter;
        private int _arg;
        private int _delay;
        private int _cycles = 4;
    }
}
