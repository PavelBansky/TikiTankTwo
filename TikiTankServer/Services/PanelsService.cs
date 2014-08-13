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
        public List<EffectData> GetEffectsInformation()
        {
            Console.WriteLine("Panels: Getting list of effects");
            return TankManager.PanelsManager.GetEffectsList();
        }

        public EffectData GetEffect()
        {
            Console.WriteLine("Panels: Getting data for active effect");
            return TankManager.PanelsManager.GetActiveEffectData();
        }
        public EffectData SetEffect(string index)
        {
            int i;
            EffectData result;

            Console.WriteLine("Panels: Setting effect to {0}", index);
            if (int.TryParse(index, out i))
            {
                result = TankManager.PanelsManager.SelectEffect(i);
                Console.WriteLine(TankManager.PanelsManager.ActiveEffect.Information.Name);
            }
            else
            {
                result = new EffectData(new EffectInfo());
                Console.WriteLine("Panels: Setting effect to {0} FAILED!", index);
            }

            return result;
        }

        public void SetColor(string color)
        {
            Console.WriteLine("Panels: Setting color to {0}", color);
            Color clr = ColorHelper.StringToColor(color);
            TankManager.PanelsManager.ActiveEffect.Color =clr;
        }

        public void SetArgument(string argument)
        {
            Console.WriteLine("Panels: Setting argument to {0}", argument);
            TankManager.PanelsManager.ActiveEffect.Argument = argument;
        }                  
    }
}
