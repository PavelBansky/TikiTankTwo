﻿using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{    
    public class AntialiasTread : IEffect
    {
        public AntialiasTread()
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
                pixels[i] = this.Color;                
            }

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

            var head = remainder; // Math.Pow(remainder, 4);
            var tail = 1 - remainder;

            for (int i = 0; i < pixels.Length; i++)
            {
                int n = (i + offset) % pixels.Length;

                switch (i % 15)
                {
                    default:
                        pixels[n] = Color.Black;
                        break;
                    case 14:
                        pixels[n] = Color.FromArgb(
                            (int)(this.Color.R * head), (int)(this.Color.G * head), (int)(this.Color.B * head));
                        break;
                    case 13: pixels[n] = this.Color;  continue;
                    case 12: pixels[n] = this.Color; continue;
                    case 11: pixels[n] = this.Color; continue;
                    case 10: pixels[n] = this.Color; continue;
                    case 9:
                        pixels[n] = Color.FromArgb(
                            (int)(this.Color.R * tail), (int)(this.Color.G * tail), (int)(this.Color.B * tail));
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


