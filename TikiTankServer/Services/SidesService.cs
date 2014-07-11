﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankCommon;

namespace TikiTankServer.Services
{
    public class SidesService : ISidesService
    {
        public void SetEffect(string index)
        {
            int i;
            Console.WriteLine("Sides: Setting effect to {0}", index);
            if (int.TryParse(index, out i))
            {
                TankManager.SidesManager.SelectEffect(i);
                Console.WriteLine(TankManager.SidesManager.ActiveEffect.Information.Name);
            }
            else
            {
                Console.WriteLine("Sides: Setting effect to {0} FAILED!", index);
            }
        }

        public void SetColor(string color)
        {
            Console.WriteLine("Sides: Setting color to {0}", color);
            Color clr = Color.ColorStringToColor(color);
            TankManager.SidesManager.ActiveEffect.SetColor(clr);
        }

        public void SetArgument(string argument)
        {
            Console.WriteLine("Sides: Setting argument to {0}", argument);
            TankManager.SidesManager.ActiveEffect.SetArgument(argument);
        }
    }
}
