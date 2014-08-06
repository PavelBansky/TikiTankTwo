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
            string spi2file = "/home/ubuntu/spidev2.0";
            if (args.Length <= 0)
            {
                HardwareHelper.SetupOverlays();
                spi2file = "/dev/spidev2.0";
            }

            LPD8806 treadsLED = new LPD8806((5 * 32) * 3, "/dev/spidev1.0");
            LPD8806 barrelLED = new LPD8806(77, spi2file);
            DMXControl dmx = new DMXControl(6);
            
            TankManager.TreadsManager.AddEffect(new SimpleTread(new EffectInfo() 
                                                   { Name = "Simple Treads", 
                                                     Description = "Gold old tread effect",
                                                     ArgumentDescription = "Speed and direction [+/-]" },
                                                     treadsLED));

            TankManager.TreadsManager.AddEffect(new Rainbow(new EffectInfo()
                                                    { Name = "Rainbow",
                                                      Description = "Running rainbow",
                                                      ArgumentDescription = "Speed"},
                                                      treadsLED));

            TankManager.TreadsManager.AddEffect(new SinWave(new EffectInfo()
                                                    { Name = "Sinus Wave",
                                                      Description = "Runing sinus wave",
                                                      ArgumentDescription = "Speed"},
                                                     treadsLED));

            TankManager.TreadsManager.AddEffect(new SolidColor(new EffectInfo()
                                                    { Name = "Solid color",
                                                      Description = "One color strip",                                                      
                                                    },
                                                    treadsLED));

            TankManager.TreadsManager.SelectEffect(0);
            Console.WriteLine("Starting Tread Manager");
            TankManager.TreadsManager.Start();

            
            TankManager.BarrelManager.AddEffect(new Rainbow(new EffectInfo()
                                                    {
                                                        Name = "Rainbow",
                                                        Description = "Running rainbow",
                                                        ArgumentDescription = "Speed"
                                                    },
                                                    barrelLED));

            TankManager.BarrelManager.AddEffect(new SinWave(new EffectInfo()
                                                    {
                                                        Name = "Sinus Wave",
                                                        Description = "Runing sinus wave",
                                                        ArgumentDescription = "Speed"
                                                    },
                                                    barrelLED));

            TankManager.BarrelManager.AddEffect(new SolidColor(new EffectInfo()
                                                    {
                                                        Name = "Solid color",
                                                        Description = "One color strip",
                                                    },
                                                    barrelLED));

            TankManager.BarrelManager.SelectEffect(0);
            Console.WriteLine("Starting Barrel Manager");
            TankManager.BarrelManager.Start();
           
            TankManager.SidesManager.AddEffect(new DMXSolidColor(new EffectInfo()
                                                { Name = "Solid color",
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
                                                   Description = "Fading brightness",
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
            TankManager.TreadsManager.Stop();
            Console.WriteLine("Stopping Barrel Manager");
            TankManager.BarrelManager.Stop();
            Console.WriteLine("Stopping Sides Manager");
            TankManager.SidesManager.Stop();
            Console.WriteLine("Stopping Nancy");
            host.Stop();  // stop hosting
            Console.WriteLine("Closing uDMX");
            dmx.Dispose();                      
        }
    }
}
