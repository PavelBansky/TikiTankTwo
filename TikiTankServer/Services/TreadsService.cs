using System;
using System.Collections.Generic;
using TikiTankCommon;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public class TreadsService : ITreadsService
    {
        public List<EffectInformation> GetEffectsInformation()
        {
            Console.WriteLine("Treads: Getting list of effects");
            return TankManager.SchedulerManager.GetEffectsInformation(DeviceType.Treads);
        }

        public void SetEffect(string index)
        {
            int i;
            Console.WriteLine("Treads: Setting effect to {0}", index);
            if (int.TryParse(index, out i))
            {
                TankManager.SchedulerManager.SelectEffect(DeviceType.Treads, i);
                //Console.WriteLine(TankManager.TreadsManager.ActiveEffect.Information.Name);
            }
            else
            {
                Console.WriteLine("Treads: Setting effect to {0} FAILED!", index);
            }
        }

/*        public void SetColor(string color)
        {
            Console.WriteLine("Treads: Setting color to {0}", color);
            Color clr = Color.ColorStringToColor(color);
            TankManager.TreadsManager.ActiveEffect.Color = clr;
        }

        public void SetArgument(string argument)
        {
            Console.WriteLine("Treads: Setting argument to {0}", argument);
            TankManager.TreadsManager.ActiveEffect.Argument =argument;
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
                TankManager.TreadsManager.ActiveEffect.IsSensorDriven = result;
                // I don't like to call this here, but I have to for now
                TankManager.TreadsManager.ActiveEffect.Activate();
            }
        }
 */
    }

}
