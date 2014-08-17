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
            int i;
            if (int.TryParse(brightness, out i))
            {
                TankManager.DmxControl.Brightness = i;
            }
        }
    }
}
