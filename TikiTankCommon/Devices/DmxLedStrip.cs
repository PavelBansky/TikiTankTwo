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
        }

        public bool Init()
        {
            return true;
        }

        public void Show(System.Drawing.Color[] pixels)
        {
            System.Diagnostics.Debug.Assert(pixels.Length == Pixels);

            short i = 0;

            foreach(Color c in pixels)
            {
                DMXControl.Instance.SetChannel(i++, (byte)gamma[c.R]);
                DMXControl.Instance.SetChannel(i++, (byte)gamma[c.G]);
                DMXControl.Instance.SetChannel(i++, (byte)gamma[c.B]);                
            }
        }

        public void Dispose()
        {            
            
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

        byte[] gamma;

    }
}
