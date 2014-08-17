using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankServer.Services
{
    public interface ISettingsService
    {
        void SetDMXBrightness(string brightness);        
    }
}
