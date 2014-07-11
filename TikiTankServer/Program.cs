using System;
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
            DMXControl dmx = new DMXControl(6);
            
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
                                                    },
                                                    treadsLED));

            TankManager.TreadManager.SelectEffect(0);
            Console.WriteLine("Starting Tread Manager");
            TankManager.TreadManager.Start();


            TankManager.SidesManager.AddEffect(new DMXSolidColor(new EffectInfo()
                                                { Name = "Color sides",
                                                  Description = "Solid color for sides",
                                                  ArgumentDescription = "Channel selector"
                                                }, dmx));

            TankManager.SidesManager.AddEffect(new Rainbow(new EffectInfo()
                                                {   Name = "Rainbow",
                                                    Description = "Running rainbow",
                                                    ArgumentDescription = "Speed"
                                                }, dmx));

            TankManager.SidesManager.AddEffect(new DMXBreath(new EffectInfo()
                                                {  Name = "Breathing",
                                                   Description = "Fading the brightness",
                                                   ArgumentDescription = "Speed"
                                                }, dmx));

            TankManager.SidesManager.SelectEffect(0);
            Console.WriteLine("Starting Sides Manager");
            TankManager.SidesManager.Start();


            Console.WriteLine("Starting Nancy self host");
            NancyHost host = new NancyHost(new Uri("http://localhost:8080"));
            host.Start();

            Console.WriteLine("Awaiting commands");
            string command = string.Empty;
            while(command != "e")
            {
                command=Console.ReadLine();
            }

            Console.WriteLine("Stopping Tread Manager");
            TankManager.TreadManager.Stop();
            Console.WriteLine("Stopping Sides Manager");
            TankManager.SidesManager.Stop();
            Console.WriteLine("Stopping Nancy");
            host.Stop();  // stop hosting
            Console.WriteLine("Closing uDMX");
            dmx.Dispose();            
        }
    }
}
