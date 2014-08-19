using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public interface ISettingsService
    {
        SettingsData GetSettings();
        void SetDMXBrightness(string brightness);

        void SetManualTick(string manualtick);

        void SetScreenSaverInterval(string interval);
    }
}
