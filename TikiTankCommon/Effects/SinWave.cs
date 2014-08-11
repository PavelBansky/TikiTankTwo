using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class SinWave : IEffect
    {
        public SinWave()
        {
            _counter = 0;
            this.Argument = "8";
            this.Color = Color.FromArgb(255, 0, 0);

        }

        public void Activate(Color[] pixels) { }

        public void Deactivate(Color[] pixels) { }

        public int Update(Color[] pixels)
        {
            Color c2 = new Color();
            float y;


            for (int i = 0; i < pixels.Length; i++)
            {
                y = (float)Math.Sin(Math.PI * (float)_cycles * (float)(_counter + i) / (float)pixels.Length);
                if (y >= 0.0)
                {
                    // Peaks of sine wave are white
                    y = 1.0F - y; // Translate Y to 0.0 (top) to 1.0 (center)
                    c2 = Color.FromArgb(
                        127 - (byte)((float)(127 - _color.R) * y),
                        127 - (byte)((float)(127 - _color.G) * y),
                        127 - (byte)((float)(127 - _color.B) * y)
                        );
                }
                else
                {
                    // Troughs of sine wave are black
                    y += 1.0F; // Translate Y to 0.0 (bottom) to 1.0 (center)
                    c2 = Color.FromArgb(
                        (byte)((float)_color.R * y),
                        (byte)((float)_color.G * y),
                        (byte)((float)_color.B * y)
                    );
                }

                pixels[i] = c2;
            }

            _counter++;
           
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
