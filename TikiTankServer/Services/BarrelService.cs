using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankCommon;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public class BarrelService : IBarrelService
    {
        public List<EffectInformation> GetEffectsInformation()
        {
            Console.WriteLine("Barrel: Getting list of effects");
            return TankManager.BarrelManager.GetEffectsInformation();
        }

        public void SetEffect(string index)
        {
            int i;
            Console.WriteLine("Barrel: Setting effect to {0}", index);
            if (int.TryParse(index, out i))
            {
                TankManager.BarrelManager.SelectEffect(i);
                Console.WriteLine(TankManager.BarrelManager.ActiveEffect.Information.Name);
            }
            else
            {
                Console.WriteLine("Barrel: Setting effect to {0} FAILED!", index);
            }
        }

        public void SetColor(string color)
        {
            Console.WriteLine("Barrel: Setting color to {0}", color);
            Color clr = Color.ColorStringToColor(color);
            TankManager.BarrelManager.ActiveEffect.Color = clr;
        }

        public void SetArgument(string argument)
        {
            Console.WriteLine("Barrel: Setting argument to {0}", argument);
            TankManager.BarrelManager.ActiveEffect.Argument =argument;
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

            Console.WriteLine("Treads: Setting sensor drive to {0} ({1})", result, sensorDrive);
            lock (this)
            {
                TankManager.BarrelManager.ActiveEffect.IsSensorDriven = result;
                // I don't like to call this here, but I have to for now
                TankManager.BarrelManager.ActiveEffect.Activate();
            }
        }        
    }    
}
