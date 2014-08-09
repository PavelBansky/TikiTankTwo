using System;
using System.Collections.Generic;

namespace TikiTankHardware
{
    public class DMXControl : LEDStrip, IDisposable
    {
        public DMXControl(int numberofZones)
            : base(numberofZones)
        {
            // Color calculator
            base.Gama = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                // http://learn.adafruit.com/light-painting-with-raspberry-pi
                base.Gama[i] = (byte)((Math.Pow((float)i / 255.0F, 2.5F) * 127.0F + 0.5F));
            }

            _dmx = new uDMX();
            if (_dmx.IsOpen)
            {
                Console.WriteLine("uDMX is connected and open");
                _dmxReady = true;
            }
            else
            {
                Console.WriteLine("uDMX NOT FOUND, it's going to be sad :-(");
                _dmxReady = false;
            }
        }

        public override void Show()
        {
            if (!_dmxReady)
                return;

            for(short i=0; i < base.Channels.Length; i++)
            {
                _dmx.SetSingleChannel(i, base.Channels[i]);
            }
        }

        public override void SetPixelRGB(int pixel, byte r, byte g, byte b)
        {
            if (pixel < Length)
            {
                // RGB
                pixel *= 3;
                Channels[pixel] = r;
                Channels[pixel + 1] = g;
                Channels[pixel + 2] = b;
            }
        }

        public override void SetPixelRGB(int pixel, byte r, byte g, byte b, byte brightness)
        {
            if (pixel < Length)
            {
                pixel *= 3;
                Channels[pixel] = Gama[(byte)(r * ((brightness / 100F) * brightness))];
                Channels[pixel + 1] = Gama[(byte)(g * ((brightness / 100F) * brightness))];
                Channels[pixel + 2] = Gama[(byte)(b * ((brightness / 100F) * brightness))];
            }
        }

        public override void SetPixelColor(int pixel, int color)
        {
            if (pixel < Length)
            {
                pixel *= 3;
                Channels[pixel] = (byte)((color >> 16));
                Channels[pixel + 1] = (byte)((color >> 8));
                Channels[pixel + 2] = (byte)(color);
            }
        }

        public override void FillRGB(int start, int count, byte r, byte g, byte b)
        {
            for (int i = start; i < start + count; i++)
            {
                SetPixelRGB(i, r, g, b);
            }
        }

        public override int GetPixelColor(int pixel)
        {
            pixel *= 3;
            return ((int)(Channels[pixel] << 16) |
                    (int)(Channels[pixel + 1] << 8) |
                    (int)Channels[pixel + 2]);
        }
        public void Dispose()
        {
            _dmx.Dispose();
        }

        private uDMX _dmx;
        private bool _dmxReady;
    }
}
