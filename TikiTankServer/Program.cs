using System;
using System.Threading.Tasks;
using Nancy.Hosting.Self;
using TikiTankCommon;
using TikiTankHardware;
using System.Threading;
using TikiTankServer.Managers;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TikiTankServer
{
    class Program
    {
        static void Main(string[] args)
          {
            HardwareHelper.SetupOverlays();
  
            TankManager.Sensor = new SpeedSensor("/dev/ttyO1");
            NancyHost host = new NancyHost(new Uri("http://localhost:8080"));
            TankManager.Sensor.Start();

            FrameScheduler scheduler = new FrameScheduler(TimeSpan.FromMilliseconds(50));

            using (StreamReader srDev = new StreamReader("Json/devices.json"))
            {
                // json device file, parse once
                var devices = JsonConvert.DeserializeObject<List<IShowable>>(
                    srDev.ReadToEnd(),
                    new DeviceConverter());

                foreach (IShowable show in devices)
                {
                    show.Init();
                    scheduler.AddDevice(show);

                }
            }

            List<IPattern> patterns;
            using (StreamReader srPat = new StreamReader("Json/patterns.json"))
            {
                // json parse patterns file
                patterns = JsonConvert.DeserializeObject<List<IPattern>>(
                    srPat.ReadToEnd(),
                    new PatternConverter());
            }          

            Console.WriteLine("Starting Nancy self host");
            host.Start();

            Console.WriteLine("Awaiting commands");
            string command = string.Empty;
            while(command != "e")
            {
                scheduler.Show(patterns); 
                //command=Console.ReadLine();
            }

            TankManager.Sensor.Stop();

            Console.WriteLine("Stopping Nancy");
            host.Stop();  // stop hosting
            Console.WriteLine("Closing uDMX");                    
        }
    }
}
