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
            HardwareHelper.SetupOverlays();
    
            LPD8806 treadsLED = new LPD8806((5 * 32) * 3, "/dev/spidev1.0");
            LPD8806 barrelLED = new LPD8806(77, "/dev/spidev2.0");
            DMXControl dmx = new DMXControl(10);
            TankManager.Sensor = new SpeedSensor("/dev/ttyO1");
            NancyHost host = new NancyHost(new Uri("http://localhost:8080"));
            
    

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
                                                      Description = "One color strip"},
                                                    treadsLED));

            TankManager.TreadsManager.SelectEffect(0);
            TankManager.TreadsManager.SelectIdleEffect(1);
            Console.Write("Tread Manager: ");
            TankManager.TreadsManager.Start();

            
            TankManager.BarrelManager.AddEffect(new Rainbow(new EffectInfo()
                                                    { Name = "Rainbow",
                                                      Description = "Running rainbow",
                                                      ArgumentDescription = "Speed"},
                                                     barrelLED));

            TankManager.BarrelManager.AddEffect(new SinWave(new EffectInfo()
                                                    { Name = "Sinus Wave",
                                                      Description = "Runing sinus wave",
                                                      ArgumentDescription = "Speed"},
                                                     barrelLED));

            TankManager.BarrelManager.AddEffect(new SolidColor(new EffectInfo()
                                                    { Name = "Solid color",
                                                      Description = "One color strip"},
                                                    barrelLED));

            TankManager.BarrelManager.SelectEffect(0);
            Console.Write("Barrel Manager: ");
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

            TankManager.SidesManager.SelectEffect(1);
            Console.Write("Sides Manager: ");
            TankManager.SidesManager.Start();


            TankManager.Sensor.Start();

            Console.WriteLine("Starting Nancy self host");
            host.Start();

            Console.WriteLine("Awaiting commands");
            string command = string.Empty;
            while(command != "e")
            {
                command=Console.ReadLine();
            }

            TankManager.Sensor.Stop();

            Console.Write("Tread Manager: ");
            TankManager.TreadsManager.Stop();
            Console.Write("Barrel Manager: ");
            TankManager.BarrelManager.Stop();
            Console.Write("Sides Manager: ");
            TankManager.SidesManager.Stop();
            Console.WriteLine("Stopping Nancy");
            host.Stop();  // stop hosting
            Console.WriteLine("Closing uDMX");
            dmx.Dispose();                      
        }
    }
}
