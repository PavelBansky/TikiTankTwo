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

        public IDisplayDevice Device;    

        public Color[] Pixels;
    }    
}
