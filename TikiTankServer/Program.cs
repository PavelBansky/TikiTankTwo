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

            DMXControl dmxControl = new DMXControl(10);
            LEDStrip treadsLED = new LEDStrip(new LPD8806((5 * 32) * 3, "/dev/spidev1.0"));            
            LEDStrip barrelLED = new LEDStrip(new LPD8806(77, "/dev/spidev2.0"));
            LEDStrip dmxLED = new LEDStrip(dmxControl);
            
            TankManager.Sensor = new SpeedSensor("/dev/ttyO1");
            NancyHost host = new NancyHost(new Uri("http://localhost:8080"));
            
    

            TankManager.TreadsManager.AddEffect(new EffectContainer(new SimpleTread(), treadsLED, 
                                                    new EffectInfo() { 
                                                        Name = "Simple Treads", 
                                                        Description = "Gold old tread effect",
                                                        ArgumentDescription = "Speed and direction [+/-]" 
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new Rainbow(), treadsLED,
                                                    new EffectInfo() { 
                                                        Name = "Rainbow",
                                                        Description = "Running rainbow",
                                                        ArgumentDescription = "Speed"
                                                    }));                                                      

            TankManager.TreadsManager.AddEffect(new EffectContainer(new SinWave(), treadsLED,
                                                    new EffectInfo() { 
                                                        Name = "Sinus Wave",
                                                        Description = "Runing sinus wave",
                                                        ArgumentDescription = "Speed"
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new SolidColor(), treadsLED,
                                                    new EffectInfo() { 
                                                        Name = "Solid color",
                                                        Description = "One color strip"
                                                    }));

            TankManager.TreadsManager.SelectEffect(0);
            TankManager.TreadsManager.SelectIdleEffect(1);
            Console.Write("Tread Manager: ");
            TankManager.TreadsManager.Start();

            
            TankManager.BarrelManager.AddEffect(new EffectContainer(new Rainbow(), barrelLED,
                                                    new EffectInfo() { 
                                                        Name = "Rainbow",
                                                        Description = "Running rainbow",
                                                        ArgumentDescription = "Speed"
                                                    }));

            TankManager.BarrelManager.AddEffect(new EffectContainer(new SinWave(), barrelLED, 
                                                    new EffectInfo() { 
                                                        Name = "Sinus Wave",
                                                        Description = "Runing sinus wave",
                                                        ArgumentDescription = "Speed"
                                                    }));

            TankManager.BarrelManager.AddEffect(new EffectContainer(new SolidColor(), barrelLED,
                                                    new EffectInfo() { 
                                                        Name = "Solid color",
                                                        Description = "One color strip"
                                                    }));


            TankManager.BarrelManager.SelectEffect(0);
            Console.Write("Barrel Manager: ");
            TankManager.BarrelManager.Start();
           
            TankManager.SidesManager.AddEffect(new EffectContainer(new DMXSolidColor(), dmxLED,
                                                    new EffectInfo() { 
                                                        Name = "Solid color",
                                                        Description = "Solid color for sides",
                                                        ArgumentDescription = "Channel selector"
                                                    }));

            TankManager.SidesManager.AddEffect(new EffectContainer(new Rainbow(), dmxLED, 
                                                    new EffectInfo() { 
                                                        Name = "Rainbow",
                                                        Description = "Running rainbow",
                                                        ArgumentDescription = "Speed"
                                                    }));

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
            dmxControl.Dispose();                      
        }
    }
}
