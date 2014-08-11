using System;
using System.Collections.Generic;
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

            for(int i=0; i < pixels.Length; i++)
            {
                short channel = (short)(i + (i * 2));
                _dmx.SetSingleChannel(channel, pixels[i].R);
                _dmx.SetSingleChannel((short)(channel + 1), pixels[i].G);
                _dmx.SetSingleChannel((short)(channel + 2), pixels[i].B);
            }
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

        private uDMX _dmx;
        private bool _dmxReady;
    }
}
