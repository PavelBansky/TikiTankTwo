using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TikiTankCommon
{
    /// <summary>
    /// Class to control generic GRB led strip
    /// </summary>
    public class LEDStrip
    {
        public LEDStrip(int numberOfLEDs)
        {
            this.Length = numberOfLEDs;
            _leds = new byte[(numberOfLEDs)*3];                

            // Color calculator
            _gamma = new byte[256];
            for(int i=0; i < 256; i++)
            {
                // http://learn.adafruit.com/light-painting-with-raspberry-pi
                _gamma[i] = (byte)(0x80 | (int)(Math.Pow((float)i / 255.0F, 2.5F) * 127.0F + 0.5F));
            }
        }

        public virtual void Show() 
        {
            throw new NotImplementedException("Show method is not implemented");
        }

        public void SetPixelRGB(int pixel, byte r, byte g, byte b)
        {
            if (pixel < Length)
            {
                // GRB
                pixel *= 3;
                _leds[pixel] = (byte)(g | 0x80);
                _leds[pixel + 1] = (byte)(r | 0x80);
                _leds[pixel + 2] = (byte)(b | 0x80);
            }
        }

        public void SetPixelRGB(int pixel, byte r, byte g, byte b, byte brightness)
        {
            if (pixel < Length)
            {
                pixel *= 3;
                _leds[pixel] = _gamma[(byte)(g * ((brightness / 100F) * brightness))];
                _leds[pixel + 1] = _gamma[(byte)(r * ((brightness / 100F) * brightness))];
                _leds[pixel + 2] = _gamma[(byte)(b * ((brightness / 100F) * brightness))];
            }
        }

        public void SetPixelColor(int pixel, int color)
        {
            if (pixel < Length)
            {
                pixel *= 3;
                _leds[pixel] = (byte)((color >> 16) | 0x80);
                _leds[pixel + 1] = (byte)((color >> 8) | 0x80);
                _leds[pixel + 2] = (byte)(color | 0x80);
            }
        }
        public void SetPixelColor(int pixel, Color color)
        {
            SetPixelRGB(pixel, color.R, color.G, color.B);
        }

        public void FillRGB(int start, int count, byte r, byte g, byte b)
        {
            for(int i=start; i < start+count; i++)
            {
                SetPixelRGB(i, r, g, b);
            }
        }

        public void FillRGB(int start, int count, Color color)
        {
            FillRGB(start, count, color.R, color.G, color.B);
        }

        public int GetPixelColor(int pixel)
        {
            pixel *= 3;
            return ((int)(_leds[pixel] & 0x7f) << 16) |
                    ((int)(_leds[pixel + 1] & 0x7f) << 8) |
                    ((int)(_leds[pixel + 2] & 0x7f));
        }

        public void RotateRight()
        {
            int lastColor = GetPixelColor(Length - 1);
            ShiftRight();
            SetPixelColor(0, lastColor);
        }

        public void RotateLeft()
        {
            int firstColor = GetPixelColor(0);
            ShiftLeft();
            SetPixelColor(this.Length - 1, firstColor);
        }

        public void ShiftRight()
        {
            for (int i = this.Length - 1; i > 0; i--)
            {
                SetPixelColor(i, GetPixelColor(i - 1));
            }

            SetPixelColor(0, 0);
        }

        public void ShiftLeft()
        {
            for (int i = 0; i < this.Length - 1; i++)
            {
                SetPixelColor(i, GetPixelColor(i + 1));
            }

            SetPixelColor(this.Length - 1, 0);
        }

        public int Length { get; set; }           

        private byte[] _gamma;
        protected byte[] _leds;
    }    
}
