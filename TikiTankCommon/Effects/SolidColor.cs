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
            startTime = DateTime.Now;
        }

        public void Activate(Color[] pixels) { }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
            TimeSpan delta = DateTime.Now - startTime;
            if (delta.TotalMilliseconds > 2000)
            {
                startTime = DateTime.Now;
                return true;
            }

            return false;
        }

        public void FrameUpdate(Color[] pixels)
        {
            StripHelper.FillColor(pixels, 0, pixels.Length, _color);
        }

        public void Tick()
        {

        }

        public bool IsSensorDriven { get; set; }
        public string Argument { get; set; }

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
        private DateTime startTime;
    }
}
