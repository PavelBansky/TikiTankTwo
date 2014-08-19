using Nancy.Hosting.Self;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TikiTankCommon;
using TikiTankCommon.Effects;
using TikiTankHardware;
using TikiTankServer.Managers;

namespace TikiTankServer
{
    class Program
    {
        static void Main(string[] args)
        {
            BeagleBoneBlack.SetupOverlays();

            TankManager.DmxControl = new DMXControl(10);
            TankManager.DmxLED = new LEDStrip(TankManager.DmxControl);   
            TankManager.TreadsLED = new LEDStrip(new LPD8806((5 * 32) * 3, "/dev/spidev1.0"));
            TankManager.BarrelLED = new LEDStrip(new LPD8806(77, "/dev/spidev2.0"));                     
            TankManager.Sensor = new SpeedSensor("/dev/ttyO1");

            TankManager.StartTheTank();

            Console.WriteLine("Starting Nancy self host");
            NancyHost host = new NancyHost( new Uri("http://localhost:8080"));
            host.Start();

            Console.WriteLine("Awaiting commands");
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            while(key.Key != ConsoleKey.Escape)
            {
                key = Console.ReadKey(true);
            }

            Console.WriteLine("Stopping Nancy");
            host.Stop();  // stop hosting                     

            TankManager.StopTheTank();
        }
    }
}
