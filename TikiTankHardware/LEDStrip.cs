using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TikiTankHardware
{
    /// <summary>
    /// Class to control generic GRB led strip
    /// </summary>
    public abstract class LEDStrip
    {
        public LEDStrip(int numberOfLEDs)
        {
            this.Length = numberOfLEDs;
            Channels = new byte[(numberOfLEDs)*3];
        }

        public abstract void Show();

        public abstract void SetPixelRGB(int pixel, byte r, byte g, byte b);

        public abstract void SetPixelRGB(int pixel, byte r, byte g, byte b, byte brightness);

        public abstract void SetPixelColor(int pixel, int color);

        public void SetPixelColor(int pixel, TikiColor color)
        {
            SetPixelRGB(pixel, color.R, color.G, color.B);
        }

        public abstract void FillRGB(int start, int count, byte r, byte g, byte b);

        public void FillRGB(int start, int count, TikiColor color)
        {
            FillRGB(start, count, color.R, color.G, color.B);
        }

        public abstract int GetPixelColor(int pixel);

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

        protected byte[] Gama;
        protected byte[] Channels;
    }    
}
