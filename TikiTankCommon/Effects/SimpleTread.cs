using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class SimpleTread : IEffect
    {
        public SimpleTread()
        {
            this.Argument = "0";
            this.Color = Color.Red;
            this.last = DateTime.Now;
            this.MetersPerTick = 10 / 39.0; // 4"

            this.metersTraveled = 0.0;
            this.metersShown = 0.0;
        }

        public void Activate(Color[] pixels)
        {
        }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
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
            double maxMeters = pixelSize * pixels.Length; // size of display (meters that can be shown)
            double traveled = metersShown; // actual offset
            while (traveled > maxMeters)
                traveled -= maxMeters; // now fits within display
            double trueOffset = traveled / (pixelSize);
            int offset = (int)(Math.Floor(trueOffset)); // offset used as pixel index
            double remainder = trueOffset - offset; // remainder used to calculate partially shaded regions
            // double overage = Math.Pow(((metersShown / pixelSize) - pixelsMoved), 4);

            for (int i = 0; i < pixels.Length; i++)
            {
                int n = (i + offset) % pixels.Length;

                switch (i % 15)
                {
                    default:
                        pixels[n] = Color.Black;
                        break;
                    case 14:
                        pixels[n] = this.Color;//Color.FromArgb(
                         //   (int)(this.Color.R * Math.Pow(remainder, 4)), (int)(this.Color.G * Math.Pow(remainder, 4)), (int)(this.Color.B * Math.Pow(remainder, 4)));
                        continue;
                    case 13: pixels[n] = this.Color; continue;
                    case 12: pixels[n] = this.Color; continue;
                    case 11: pixels[n] = this.Color;  continue;
                    case 10:
                        pixels[n] = this.Color;
                        continue;
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


