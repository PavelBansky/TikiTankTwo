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
            BeagleBoneBlack.SetupOverlays();

            DMXControl dmxControl = new DMXControl(10);
            LEDStrip treadsLED = new LEDStrip(new LPD8806((5 * 32) * 3, "/dev/spidev1.0"));
            LEDStrip barrelLED = new LEDStrip(new LPD8806(77, "/dev/spidev2.0"));
            LEDStrip dmxLED = new LEDStrip(dmxControl);
            
            TankManager.Sensor = new SpeedSensor("/dev/ttyO1");
            NancyHost host = new NancyHost(new Uri("http://localhost:8080"));
            
    
            TankManager.TreadsManager.AddEffect(new EffectContainer(new SimpleTread(), treadsLED, 
                                                    new EffectInfo() { 
                                                        Name = "Simple Treads", 
                                                        Description = "Good old tread effect",
                                                        ArgumentDescription = "Speed and direction [+/-]" 
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new RainbowTread(), treadsLED,
                                                    new EffectInfo()
                                                    {
                                                        Name = "Rainbow Treads",
                                                        Description = "Rainbow tread effect",
                                                        ArgumentDescription = "Speed and direction [+/-]"
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new Rainbow(), treadsLED,
                                                    new EffectInfo() { 
                                                        Name = "Rainbow",
                                                        Description = "Running rainbow",
                                                        ArgumentDescription = "Speed"
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new CameraFlashes(), treadsLED,
                                                    new EffectInfo()
                                                    {
                                                        Name = "Camera Flashes",
                                                        Description = "Camera flashes",
                                                        ArgumentDescription = "Flashes per second"
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new SinWave(), treadsLED,
                                                    new EffectInfo() { 
                                                        Name = "Sin Wave",
                                                        Description = "Runing sinus wave",
                                                        ArgumentDescription = "Speed"
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new Glow(), treadsLED,
                                                    new EffectInfo()
                                                    {
                                                        Name = "Glow",
                                                        Description = "Glowing"
                                                    }));

            TankManager.TreadsManager.AddEffect(new EffectContainer(new SolidColor(), treadsLED,
                                                    new EffectInfo()
                                                    {
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

            TankManager.BarrelManager.AddEffect(new EffectContainer(new CameraFlashes(), barrelLED,
                                                    new EffectInfo()
                                                    {
                                                        Name = "Camera Flashes",
                                                        Description = "Camera flashes",
                                                        ArgumentDescription = "Flashes per second"
                                                    }));

            TankManager.BarrelManager.AddEffect(new EffectContainer(new SinWave(), barrelLED, 
                                                    new EffectInfo() { 
                                                        Name = "Sin Wave",
                                                        Description = "Runing sinus wave",
                                                        ArgumentDescription = "Speed"
                                                    }));

            TankManager.BarrelManager.AddEffect(new EffectContainer(new Glow(), barrelLED,
                                                    new EffectInfo() {
                                                        Name = "Glow",
                                                        Description = "Glowing"
                                                    }));

            TankManager.BarrelManager.AddEffect(new EffectContainer(new SolidColor(), barrelLED,
                                                    new EffectInfo() { 
                                                        Name = "Solid color",
                                                        Description = "One color strip"
                                                    }));


            TankManager.BarrelManager.SelectEffect(0);
            Console.Write("Barrel Manager: ");
            TankManager.BarrelManager.Start();

            TankManager.PanelsManager.AddEffect(new EffectContainer(new DMXRainbow(), dmxLED,
                                                    new EffectInfo()
                                                    {
                                                        Name = "Rainbow",
                                                        Description = "Clasic rainbow",
                                                        ArgumentDescription = "Speed"
                                                    }));

            TankManager.PanelsManager.AddEffect(new EffectContainer(new DMXSolidColor(), dmxLED,
                                                    new EffectInfo() { 
                                                        Name = "Solid color",
                                                        Description = "Solid color for sides",
                                                        ArgumentDescription = "Channel selector"
                                                    }));

            TankManager.PanelsManager.AddEffect(new EffectContainer(new DMXGlow(), dmxLED,
                                                    new EffectInfo()
                                                    {
                                                        Name = "Glow",
                                                        Description = "Glowing"
                                                    }));


            TankManager.PanelsManager.SelectEffect(0);
            Console.Write("Sides Manager: ");
            TankManager.PanelsManager.Start();

            TankManager.Sensor.Start();

            Console.WriteLine("Starting Nancy self host");
            host.Start();

            Console.WriteLine("Awaiting commands");
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while(key.Key != ConsoleKey.Escape)
            {
                key = Console.ReadKey(true);
            }

            TankManager.Sensor.Stop();

            Console.Write("Tread Manager: ");
            TankManager.TreadsManager.Stop();
            Console.Write("Barrel Manager: ");
            TankManager.BarrelManager.Stop();
            Console.Write("Sides Manager: ");
            TankManager.PanelsManager.Stop();
            Console.WriteLine("Stopping Nancy");
            host.Stop();  // stop hosting
            Console.WriteLine("Closing uDMX");
            dmxControl.Dispose();                      
        }
    }
}
