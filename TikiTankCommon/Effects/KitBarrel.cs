using System.Runtime.Serialization;
using System.Drawing;
using System;

namespace TikiTankCommon.Effects
{
	public class KitBarrel : IBarrelEffect
    {
        public KitBarrel()
        {
            Color = Color.White;
        }

        int kitLen = 11;

        public void Activate(System.Drawing.Color[] pixels)
        {
            pos = 0;
            right = true;

            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = Color.Black;

            pixels[0] = Color.Red.MakeDarker(0.75f);
			pixels[1] = Color.Red.MakeDarker(0.50f);
			pixels[2] = Color.Red.MakeDarker(0.25f);
			pixels[3] = Color.Red.MakeDarker(0.10f);
			pixels[4] = Color.Red;
            pixels[5] = Color.Red;
			pixels[6] = Color.Red.MakeDarker(0.10f);
			pixels[7] = Color.Red.MakeDarker(0.25f);
			pixels[8] = Color.Red.MakeDarker(0.50f);
			pixels[9] = Color.Red.MakeDarker(0.75f);
        }

        public void Deactivate(System.Drawing.Color[] pixels) { }

        DateTime last = DateTime.Now;
        public bool WouldUpdate()
        {
            var span = DateTime.Now - last;
            if (span.TotalMilliseconds >= 70)
            {
                last = DateTime.Now;
                return true;
            }

            return false;
        }

        // move from 0 -> max len

        int pos = 0;
        bool right = true;

        public void FrameUpdate(Color[] pixels)
        {
			//Console.WriteLine("test");

            if (right)
            {
                StripHelper.ShiftRight(pixels);
                pos += 1;
                if (pos >= pixels.Length - kitLen)
                    right = false;
            }
            else
            {
                StripHelper.ShiftLeft(pixels);
                pos -= 1;
                if (pos < 0)
                    right = true;
            }
        }

        public void Tick() { }
        public bool IsSensorDriven { get; set; }
        public string Argument { get; set; }
        public System.Drawing.Color Color { get; set; }
    }
}