using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class SolidColor : IEffect
    {
        public SolidColor()
        {
            this.Color = Color.Red;
        }

        public void Activate(Color[] pixels) { }

        public void Deactivate(Color[] pixels) { }

        public int Update(Color[] pixels)
        {
            StripHelper.FillColor(pixels, 0, pixels.Length, _color);
            //LedStrip.Show();
            // Refresh every second
            return 2000;
        }

        public string Argument
        {
            get;
            set;
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
    }
}
