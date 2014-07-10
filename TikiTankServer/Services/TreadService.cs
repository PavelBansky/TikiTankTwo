using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankCommon;

namespace TikiTankServer.Services
{
    public class TreadService : ITreadService
    {
        public void SetEffect(string index)
        {
            int i;
            Console.WriteLine("Treads: Setting effect to {0}", index);
            if (int.TryParse(index, out i))
            {
                TankManager.TreadManager.SelectEffect(i);
                Console.WriteLine(TankManager.TreadManager.ActiveEffect.Information.Name);
            }
            else
            {
                Console.WriteLine("Treads: Setting effect to {0} FAILED!", index);
            }
        }

        public void SetColor(string color)
        {
            Console.WriteLine("Treads: Setting color to {0}", color);
            Color clr = Color.ColorStringToColor(color);
            TankManager.TreadManager.ActiveEffect.SetColor(clr);
        }

        public void SetArgument(string argument)
        {
            Console.WriteLine("Treads: Setting argument to {0}", argument);
            TankManager.TreadManager.ActiveEffect.SetArgument(argument);
        }        
    }

}
