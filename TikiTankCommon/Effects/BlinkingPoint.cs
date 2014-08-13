using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    class BlinkingPoint : IEffect
    {
        public BlinkingPoint()
        {
            this.Color = System.Drawing.Color.Red;             
        }

        public void Activate(System.Drawing.Color[] pixels)
        {
            startTime = DateTime.Now;     
            length = pixels.Length;
            position = 0;
            pointVisible = true;
        }

        public void Deactivate(System.Drawing.Color[] pixels)
        {

        }

        public void FrameUpdate(System.Drawing.Color[] pixels)
        {
            // Clean the strip
            for (int i = 0; i < length; i++)
                pixels[i] = System.Drawing.Color.Black;

            // Show the point
            if (pointVisible)
                pixels[position] = this.Color;

            pointVisible = !pointVisible;
        }

        public bool WouldUpdate()
        {
            // Determine if it's time to update based (every 100 millisecond)
            TimeSpan delta = DateTime.Now - startTime;
            if (delta.TotalMilliseconds > 100)
            {
                startTime = DateTime.Now;
                return true;
            }

            return false;
        }

        public void Tick()
        {
            // Move the point on tick
            position++;
            if (position >= length)
                position = 0;
        }

        public bool IsSensorDriven { get; set; }

        public string Argument { get; set; }

        public System.Drawing.Color Color { get; set; }

        private int position;
        private int length;
        private bool pointVisible;
        private DateTime startTime;
    }
}
