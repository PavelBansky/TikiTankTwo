using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TikiTankCommon;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public class PanelsService : IPanelsService
    {
        public List<EffectInformation> GetEffectsInformation()
        {
            Console.WriteLine("Panels: Getting list of effects");
            return TankManager.SidesManager.GetEffectsInformation();
        }
    
        public void SetEffect(string index)
        {
            int i;
            Console.WriteLine("Panels: Setting effect to {0}", index);
            if (int.TryParse(index, out i))
            {
                TankManager.SidesManager.SelectEffect(i);
                Console.WriteLine(TankManager.SidesManager.ActiveEffect.Information.Name);
            }
            else
            {
                Console.WriteLine("Panels: Setting effect to {0} FAILED!", index);
            }
        }

        public void SetColor(string color)
        {
            Console.WriteLine("Panels: Setting color to {0}", color);
            Color clr = ColorHelper.StringToColor(color);
            TankManager.SidesManager.ActiveEffect.Color =clr;
        }

        public void SetArgument(string argument)
        {
            Console.WriteLine("Panels: Setting argument to {0}", argument);
            TankManager.SidesManager.ActiveEffect.Argument = argument;
        }

        public void SetSensorDrive(string sensorDrive)
        {
            bool result = false;
            string arg = sensorDrive.ToUpper();
            if (arg == true.ToString().ToUpper() || arg == "1")
            {
                result = true;
            }
            else
            {
                result = false;
            }

            Console.WriteLine("Panels: Setting sensor drive to {0} ({1})", result, sensorDrive);
            lock (this)
            {
                TankManager.SidesManager.ActiveEffect.IsSensorDriven = result;
                // I don't like to call this here, but I have to for now
                TankManager.SidesManager.ActiveEffect.Activate();
            }
        }                    
    }
}
