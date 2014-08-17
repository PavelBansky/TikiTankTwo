using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{    
    public class SimpleTread : IEffect
    {
        public SimpleTread()
        {         
            this.Argument ="0";
            this.Color = Color.White;            
            this.last = DateTime.Now;
            this.counter = 0;
            this.rainbowIncrease = true;
            this.MetersPerTick = 10 / 39.0; // 4"

            this.metersTraveled = 0.0;
            this.metersShown = 0.0;
        }

        public void Activate(Color[] pixels)
        {
            StripHelper.FillColor(pixels, 0, pixels.Length, Color.Black);
            startIndex = 0;
            memory = new Color[15];
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
            metersShown = (metersShown * 30 + metersTraveled) / 31;

            double pixelSize = 1.0 / 32; // 32 pixels per meter
            // # of pixels "moved" forward modulo # of pixels in display --> how many pixels to rotate design
            int pixelsMoved = (int)(metersShown / pixelSize);
            double overage = Math.Pow(((metersShown / pixelSize) - pixelsMoved), 2);
            double overage2 = Math.Pow(overage, 2);

           // Console.WriteLine("leading (overage): {0}, {1}", overage, overage2);
            int offset = pixelsMoved % pixels.Length;
            for (int i = 0; i < pixels.Length; i++)
            {
                int n = (i + offset) % pixels.Length;

                switch (i % 16)
                {
                    default:
                        pixels[n] = Color.Black;
                        break;
                    case 15:
                        pixels[n] = Color.FromArgb(
                            (int)(this.Color.R * overage2), (int)(this.Color.G * overage2), (int)(this.Color.B * overage2));
                        break;

                    case 14: continue;
                    case 13: continue;
                    case 12: continue;
                    case 11: continue;
                    case 10:
                        pixels[n] = this.Color;
                        break;
                    case 9:
                        pixels[n] = Color.FromArgb(
                            (int)(this.Color.R * overage), (int)(this.Color.G * overage), (int)(this.Color.B * overage));
                        break;
                    case 8:
                        pixels[n] = Color.FromArgb(
                            (int)(this.Color.R * overage2), (int)(this.Color.G * overage2), (int)(this.Color.B * overage2));
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

        private bool rainbowIncrease;
        private int counter;
        private Color pixelColor;
        private bool rainbowEnabled;
        private DateTime last;
        private int startIndex;      
        private int Period;
        private Color[] memory;
        private double metersTraveled;
        private double metersShown;
        private double MetersPerTick;

    }
     
}


