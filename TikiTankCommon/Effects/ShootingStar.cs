using System.Runtime.Serialization;
using System.Drawing;
using System;

namespace TikiTankCommon.Effects
{
	public class ShootingStar : IBarrelEffect
    {
        public ShootingStar()
        {
            Color = Color.White;
        }


        public void Activate(System.Drawing.Color[] pixels)
        {
             pos = 0;
        }

        public void Deactivate(System.Drawing.Color[] pixels) { }

        DateTime last = DateTime.Now;
        public bool WouldUpdate()
        {
            var span = DateTime.Now - last;
            if (span.TotalMilliseconds >= 10)
            {
                last = DateTime.Now;
                return true;
            }
            return false;
        }
        
        // move from 0 -> max len

        int pos = 0;
        int lineLen = 10;

        public void FrameUpdate(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = Color.Black;

            for (int i = pos, x = 0; i > -1 && x < lineLen; i--, x++)
                pixels[i] = Color.FromArgb(
                        Color.R / (x+1), Color.G / (x+1), Color.B / (x+1));

            pos += 1;
            
            if (pos >= pixels.Length) pos = 0;
        }

        public void Tick() { }
        public bool IsSensorDriven { get; set; }
        public string Argument { get; set; }
        public System.Drawing.Color Color { get; set; }
    }
}