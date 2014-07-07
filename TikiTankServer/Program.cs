﻿using System;
using System.Threading.Tasks;
using Nancy.Hosting.Self;
using TikiTankCommon.Effects;
using TikiTankCommon;
using TikiTankHardware;
using System.Threading;

namespace TikiTankServer
{
    class Program
    {

        static void Main(string[] args)
        {

            LPD8806 treadsLED = new LPD8806((5 * 32) * 3, "/dev/spidev1.0");
            
            TankManager.TreadManager.AddEffect(new SimpleTread(new EffectInfo() 
                                                   { Name = "Simple Treads", 
                                                     Description = "Gold old tread effect",
                                                     ArgumentDescription = "Speed and direction [+/-]" },
                                                     treadsLED));

            TankManager.TreadManager.AddEffect(new Rainbow(new EffectInfo()
                                                    { Name = "Rainbow",
                                                      Description = "Running rainbow",
                                                      ArgumentDescription = "Speed"},
                                                      treadsLED));

            TankManager.TreadManager.AddEffect(new SinWave(new EffectInfo()
                                                    { Name = "Sinus Wave",
                                                      Description = "Runing sinus wave",
                                                      ArgumentDescription = "Speed"},
                                                     treadsLED));

            TankManager.TreadManager.AddEffect(new SolidColor(new EffectInfo()
                                                    { Name = "Solid color",
                                                      Description = "One color strip",
                                                      ArgumentDescription = ""
                                                    },
                                                    treadsLED)); 


            TankManager.TreadManager.SelectEffect(0);


            Console.WriteLine("Starting Nancy self host");
            NancyHost host = new NancyHost(new Uri("http://localhost:8080"));
            host.Start(); // start hosting

            Console.WriteLine("Starting main loop");
            int delay = 0;
            DateTime startTime = DateTime.Now;
            while(true)
            {
                TimeSpan delta = DateTime.Now - startTime;                     
                if (delta.TotalMilliseconds >= delay )
                {
                    delay = TankManager.TreadManager.ActiveEffect.Step();
                    startTime = DateTime.Now;
                }                
            }
            
            host.Stop();  // stop hosting
        }
    }
}
