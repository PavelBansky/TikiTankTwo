using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class SolidColor : Effect
    {
        public SolidColor(EffectInfo info, LEDStrip strip)
        {
            this.Information = info;
            this.LedStrip = strip;                        
            SetColor(Color.FromRgb(255, 0, 0));
        }

        public override void Activate()
        {
            
        }

        public override void Deactivate()
        {
            
        }

        public override int Step()
        {
            LedStrip.FillRGB(0, LedStrip.Length, _color);
            LedStrip.Show();
            // Refresh every second
            return 2000;
        }

        public override void SetArgument(string argument)
        {
            //
        }

        public override string GetArgument()
        {
            return string.Empty;
        }

        public override void SetColor(Color color)
        {
            _color = color;
        }

        public override Color GetColor()
        {
            return this._color;
        }

        private Color _color;
    }
}
