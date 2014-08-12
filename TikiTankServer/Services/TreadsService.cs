using System;
using System.Collections.Generic;
using System.Drawing;
using TikiTankCommon;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public class TreadsService : ITreadsService
    {
        public List<EffectData> GetEffectsInformation()
        {
            Console.WriteLine("Treads: Getting list of effects");
            return TankManager.TreadsManager.GetEffectsInformation();
        }

        public EffectData SetEffect(string index)
        {
            int i;
            EffectData result;

            Console.WriteLine("Treads: Setting effect to {0}", index);
            if (int.TryParse(index, out i))
            {
                result = TankManager.TreadsManager.SelectEffect(i);
                Console.WriteLine(TankManager.TreadsManager.ActiveEffect.Information.Name);

            }
            else
            {
                result = new EffectData(new EffectInfo());
                Console.WriteLine("Treads: Setting effect to {0} FAILED!", index);
            }

            return result;
        }

        public void SetColor(string color)
        {
            Console.WriteLine("Treads: Setting color to {0}", color);
            Color clr = ColorHelper.StringToColor(color);
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
    }

}
