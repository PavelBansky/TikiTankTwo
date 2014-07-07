using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankCommon;

namespace TikiTankServer
{
    public static class TankManager
    {
        static TankManager()
        {
            TreadManager = new EffectManager();
            BarrelManager = new EffectManager();
            SidesManager = new EffectManager();            
        }

        public static EffectManager TreadManager { get; set; }
        public static EffectManager BarrelManager { get; set; }
        public static EffectManager SidesManager { get; set; }
    }
}
