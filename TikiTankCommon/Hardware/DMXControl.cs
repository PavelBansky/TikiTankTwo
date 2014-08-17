using System;
using System.Drawing;
using TikiTankCommon;

namespace TikiTankHardware
{
    public class DMXControl : IDisplayDevice
    {
        public DMXControl(int length)
        {
            this.Length = length;

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

        public void Show(Color[] pixels)
        {
            if (!_dmxReady)
                return;            

            SetPixelColor(5, pixels[0]);
            SetPixelColor(3, pixels[1]);
            SetPixelColor(4, pixels[2]);
            SetPixelColor(9, pixels[3]);
            SetPixelColor(8, pixels[4]);
            SetPixelColor(6, pixels[5]);
            SetPixelColor(7, pixels[6]);
            SetPixelColor(2, pixels[7]);
            SetPixelColor(1, pixels[8]);
            SetPixelColor(0, pixels[9]);
        }
        
        private void SetPixelColor(short pixel, Color color)
        {
            pixel *= 3;
            int brth = 100 / _brightness; ;
            _dmx.SetSingleChannel(pixel, (byte)(color.R / brth));
            _dmx.SetSingleChannel((short)(pixel+1), (byte)(color.G / brth));
            _dmx.SetSingleChannel((short)(pixel+2), (byte)(color.B / brth));
        }

        public void Dispose()
        {
            _dmx.Dispose();
        }

        public int Length
        {
            get;
            private set;
        }

        public int Brightness
        {
            get { return _brightness; }
            set 
            {
                if (value > 0 && value <= 100)
                {
                    _brightness = value;
                }
            }
        }

        private int _brightness;
        private uDMX _dmx;
        private bool _dmxReady;
    }
}
