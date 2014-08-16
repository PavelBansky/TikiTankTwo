using System.Drawing;
using TikiTankHardware;

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

        public IDisplayDevice Device { get; set; }

        public Color[] Pixels { get; set; }
    }    
}
