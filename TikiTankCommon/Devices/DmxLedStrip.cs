using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TikiTankHardware;

namespace TikiTankCommon.Devices
{
    public class DmxLedStrip : IShowable, IDisposable
    {
        public DmxLedStrip()
        {
            gamma = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                // http://learn.adafruit.com/light-painting-with-raspberry-pi
                gamma[i] = (byte)((Math.Pow((float)i / 255.0F, 2.5F) * 127.0F + 0.5F));
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

        public bool Init()
        {
            return true;
        }

        public void Show(System.Drawing.Color[] pixels)
        {
            System.Diagnostics.Debug.Assert(pixels.Length == Pixels);

            if (!_dmxReady)
                return;

            short i = 0;

            foreach(Color c in pixels)
            {
                _dmx.SetSingleChannel(i++, (byte)gamma[c.R]);
                _dmx.SetSingleChannel(i++, (byte)gamma[c.G]);
                _dmx.SetSingleChannel(i++, (byte)gamma[c.B]);
                
            }
        }

        public void Dispose()
        {
            _dmx.Dispose();
        }

        [DataMember]
        public string Name
        {
            get; set;
        }

        [DataMember]
        public int Pixels
        {
            get; set;
        }

        private uDMX _dmx;
        private bool _dmxReady;

        byte[] gamma;

    }
}
