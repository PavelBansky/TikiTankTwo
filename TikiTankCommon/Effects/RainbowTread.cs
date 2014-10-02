using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{    
    public class RainbowTread : IEffect
    {
        public RainbowTread()
        {         
            this.Argument ="0";
            this.Color = Color.White;            
            this.last = DateTime.Now;                        
            this.MetersPerTick = 10 / 39.0; // 4"

            this.metersTraveled = 0.0;
            this.metersShown = 0.0;
        }

        public void Activate(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[(pixels.Length - 1) - i] = ColorHelper.Wheel(((i * 384 / pixels.Length)) % 384);
            }

        }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
            // implement static speed control, turn to 0 to rely on interrupts
            if (!IsSensorDriven && Period > 0)
            {
                if ((DateTime.Now - last).TotalMilliseconds > Period)
                {
                    last = DateTime.Now;
                    Tick();
                }
            }

            if (metersShown > metersTraveled)
                return false;

            return true;
        }

        public void FrameUpdate(Color[] pixels)
        {

            // move the treads about 5% of the way toward the goal each frame
            // while this does lag a bit, it is so simple that it's attractive
            // and it speeds up exponentially rather than continuing to lag
            metersShown = (metersShown * SimpleTread.SmoothFactor + metersTraveled) / (SimpleTread.SmoothFactor + 1);
            double pixelSize = 1.0 / 32; // 32 pixels per meter
            double maxMeters = pixelSize * pixels.Length; // size of display (meters that can be shown)
            double traveled = metersShown; // actual offset
            while (traveled > maxMeters)
                traveled -= maxMeters; // now fits within display
            double trueOffset = traveled / ( pixelSize); 
            int offset = (int)(Math.Floor(trueOffset)); // offset used as pixel index
            double remainder = trueOffset - offset; // remainder used to calculate partially shaded regions
            // double overage = Math.Pow(((metersShown / pixelSize) - pixelsMoved), 4);
            //Console.WriteLine("offset: {0}", remainder);

            var head = remainder; // Math.Pow(remainder, 4);
            var tail = 1 - remainder;

           // for (int i = 0; i < pixels.Length; i++)
           // {
           //     pixels[(pixels.Length - 1) - i] = ColorHelper.Wheel(((i * 384 / pixels.Length)) % 384);
           // }

            for (int i = 0; i < pixels.Length; i++)
            {
                int c = (int)((double)i / (double)pixels.Length * 383);
                pixels[(pixels.Length - 1) - i] = ColorHelper.Wheel(c);
            }

            for (int i = 0; i < pixels.Length; i++)
            {
                int n = (i + offset) % pixels.Length;
                var c = pixels[n];

                switch (i % 15)
                {
                    default:
                        pixels[n] = Color.Black;
                        break;
                    case 14:
                        pixels[n] = Color.FromArgb(
                            (int)(c.R * head), (int)(c.G * head), (int)(c.B * head));
                        break;
                    case 13: pixels[n] = c;  continue;
                    case 12: pixels[n] = c; continue;
                    case 11: pixels[n] = c; continue;
                    case 10: pixels[n] = c; continue;
                    case 9:
                        pixels[n] = Color.FromArgb(
                            (int)(c.R * tail), (int)(c.G * tail), (int)(c.B * tail));
                        break;
                }
            }           
        }

        public void Tick()
        {
            // 4 inches per tick, in meters
            metersTraveled += MetersPerTick;
        }

        public bool IsSensorDriven { get; set; }

        public Color Color { get; set; }

        public string Argument
        {
            get
            {
                return Period.ToString();
            }
            set 
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    Period = i;
                }
            }
        }
                
        private DateTime last;            
        private int Period;
        //private Color[] memory;
        private double metersTraveled;
        private double metersShown;
        private double MetersPerTick;

    }
     
}


