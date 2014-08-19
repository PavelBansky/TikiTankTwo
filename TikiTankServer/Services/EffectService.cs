using System;
using System.Collections.Generic;
using System.Drawing;
using TikiTankCommon;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public class EffectService : IEffectService
    {
        public List<Managers.EffectData> GetEffectsInformation(string device)
        {
            Console.WriteLine("{0}: Getting list of effects", device);
            return GetEffectManager(device).GetEffectsList();
        }

        public EffectData SetEffect(string device, string index)
        {
            int i;
            EffectData result;

            Console.WriteLine("{0}: Setting effect to {1}", device, index);
            if (int.TryParse(index, out i))
            {
                result = GetEffectManager(device).SelectEffect(i);
                Console.WriteLine(TankManager.TreadsManager.ActiveEffect.Information.Name);

            }
            else
            {
                result = new EffectData(new EffectInfo());
                Console.WriteLine("{0}: Setting effect to {1} FAILED!", device, index);
            }

            return result;
        }

        public EffectData GetEffect(string device)
        {
            Console.WriteLine("{0}: Getting data for active effect", device);
            return GetEffectManager(device).GetActiveEffectData();
        }

        public void SetColor(string device, string color)
        {
            Console.WriteLine("{0}: Setting color to {1}", device, color);
            Color clr = ColorHelper.StringToColor(color);
            GetEffectManager(device).ActiveEffect.Color = clr;
        }

        public void SetArgument(string device, string argument)
        {
            Console.WriteLine("{0}: Setting argument to {1}", device, argument);
            GetEffectManager(device).ActiveEffect.Argument = argument;
        }

        public void SetSensorDrive(string device, string sensorDrive)
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

            Console.WriteLine("{0}: Setting sensor drive to {1} ({2})", device, result, sensorDrive);
            GetEffectManager(device).SetSensorDrive(result);
        }

        public void SetAsScreenSaver(string device, string screenSaver)
        {
            bool result = false;
            string arg = screenSaver.ToUpper();
            if (arg == true.ToString().ToUpper() || arg == "1")
            {
                result = true;
            }
            else
            {
                result = false;
            }

            Console.WriteLine("{0}: Setting screen saver to {1} ({2})", device, result, screenSaver);
            GetEffectManager(device).SetAsScreenSaver(result);
        }

        private EffectManager GetEffectManager(string device)
        {
            EffectManager result;
            switch (device)
            {
                case "barrel":
                    result = TankManager.BarrelManager;
                    break;
                case "panels":
                    result = TankManager.PanelsManager;
                    break;
                default:
                    result = TankManager.TreadsManager;
                    break;
            }

            return result;
        }
    }
}
