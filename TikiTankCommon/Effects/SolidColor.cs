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
            this.Color = Color.FromRgb(255, 0, 0);
        }

        public override void Activate() { }

        public override void Deactivate() { }

        public override int Step()
        {
            LedStrip.FillRGB(0, LedStrip.Length, _color);
            LedStrip.Show();
            // Refresh every second
            return 2000;
        }

        public override string Argument
        {
            get;
            set;
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
    }
}
