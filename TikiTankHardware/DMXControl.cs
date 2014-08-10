using System;
using System.Collections.Generic;

namespace TikiTankHardware
{
    public class DMXControl : IDisposable
    {
        public DMXControl()
        {
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

        public void SetChannel(int channel, int value)
        {
            if (_dmxReady)
                _dmx.SetSingleChannel((short)channel, (byte)value);
        }

        public void Dispose()
        {
            _dmx.Dispose();
        }

        private uDMX _dmx;
        private bool _dmxReady;

        private static DMXControl _instance;
        public static DMXControl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DMXControl();
               
                return _instance;
            }
        }
    }
}
