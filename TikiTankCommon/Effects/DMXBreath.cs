using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class DMXBreath : Effect
    {
        public DMXBreath(EffectInfo info, LEDStrip strip)
        {
            this.Information = info;
            this.LedStrip = strip;
            _colors = new Color[strip.Length];
            _step = 0;
            _increase = false;
        }

        public override void Activate()
        {
            for(int i=0; i < _colors.Length; i++)
            {
                _colors[i] = Color.FromRgb(LedStrip.GetPixelColor(i));
            }
        }

        public override void Deactivate()
        {
            //
        }

        public override int Step()
        {
            int brightnes = ((_increase)? 50 : 100)-(((_increase)? -1 : 1)*_step*10);
            if (_step++ > 5)
            {
                _step = 0;
                _increase = !_increase;
            }

            for (int i = 0; i < LedStrip.Length; i++)
            {
                LedStrip.SetPixelRGB(i, _colors[i].R, _colors[i].G, _colors[i].B, (byte)brightnes);
                LedStrip.Show();
            }

            return 50;
        }

        public override void SetArgument(string argument)
        {
            int a;
            if (int.TryParse(argument, out a))
            {
                if (a >=0 && a < _colors.Length)
                {
                    _arg = a;
                }
            }
        }

        public override string GetArgument()
        {
            return _arg.ToString();
        }

        public override void SetColor(Color color)
        {
            _colors[_arg] = color;
        }

        public override Color GetColor()
        {
            return _colors[_arg];
        }

        private int _step;
        private bool _increase;
        private int _arg;
        private Color[] _colors;
    }
}
