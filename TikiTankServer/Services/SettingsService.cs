using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public class SettingsService : ISettingsService
    {
        public SettingsData GetSettings()
        {
            Console.WriteLine("Settings: Getting settings");
            return TankManager.GetSettings();
        }

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
            Console.WriteLine("Settings: Setting screen saver effect inetrval {0}", interval);
            int i;
            if (int.TryParse(interval, out i))
            {
                TankManager.SetScreenSaverInterval(i);
            }
        }
    }
}
