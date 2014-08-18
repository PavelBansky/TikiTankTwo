using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankServer.Services
{
    public class SettingsService : ISettingsService
    {
        public void SetDMXBrightness(string brightness)
        {
            Console.WriteLine("Settings: Setting DMX brightness {0}", brightness);
            int i;
            if (int.TryParse(brightness, out i))
            {
                TankManager.DmxControl.Brightness = i;
            }
        }

        public void SetManualTick(string manualtick)
        {
            Console.WriteLine("Settings: Setting manual tick {0}", manualtick);
            int i;
            if (int.TryParse(manualtick, out i))
            {
                TankManager.SetManualTick(i);
            }
        }

        public void SetScreenSaverInterval(string interval)
        {
            //throw new NotImplementedException();
        }
    }
}
