﻿using System;
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

        public override void Activate()
        {

        }

        public override void Deactivate()
        {
            //
        }

        public override int Step()
        {
            LedStrip.Show();

            return 2000;
        }

        public override void SetArgument(string argument)
        {
            int a;
            if (int.TryParse(argument, out a))
            {
                if (a >=0 && a < LedStrip.Length)
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
            LedStrip.SetPixelColor(_arg, color);
        }

        public override Color GetColor()
        {
            return Color.FromRgb(LedStrip.GetPixelColor(_arg));
        }

        private int _arg;
    }
}
