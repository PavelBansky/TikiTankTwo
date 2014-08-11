using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace TikiTankCommon
{
    /// <summary>
    /// Class to control generic GRB led strip
    /// </summary>
    public class LEDStrip
    {
        public LEDStrip(IDisplayDevice device)
        {            
            this.Device = device;
            Pixels = new Color[device.Length];
        }

        public void Show()
        {
            Device.Show(Pixels);
        }

        public void SetPixelRGB(int pixel, byte r, byte g, byte b)
        {
            if (pixel < Pixels.Length)
            {
                Pixels[pixel] = Color.FromArgb(r, g, b);
            }
        }

        public void SetPixelColor(int pixel, Color color)
        {
            SetPixelRGB(pixel, color.R, color.G, color.B);
        }
/*
        public override void SetPixelRGB(int pixel, byte r, byte g, byte b, byte brightness)
        {
            if (pixel < Length)
            {
                pixel *= 3;
                Channels[pixel] = Gama[(byte)(g * ((brightness / 100F) * brightness))];
                Channels[pixel + 1] = Gama[(byte)(r * ((brightness / 100F) * brightness))];
                Channels[pixel + 2] = Gama[(byte)(b * ((brightness / 100F) * brightness))];
            }
        }
        */

        public IDisplayDevice Device;    

        public Color[] Pixels;
    }    
}
