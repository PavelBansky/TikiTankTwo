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
using TikiTankCommon.Converters;

namespace TikiTankServer
{
    class Program
    {
        static void Main(string[] args)
          {
            HardwareHelper.SetupOverlays();
  
            TankManager.Sensor = new SpeedSensor("/dev/ttyO1");
            NancyHost host = new NancyHost(new Uri("http://localhost:8080"));
            DMXControl.Instance.SetChannel(0, 0);

            TankManager.Sensor.Start();

            TankManager.SchedulerManager.Start();

            Console.WriteLine("Starting Nancy self host");
            host.Start();
            Console.WriteLine("Awaiting commands");


            string command = string.Empty;
            
            while(command != "e")
            {                
                command=Console.ReadLine();
            }


            TankManager.SchedulerManager.Stop();
            TankManager.Sensor.Stop();

            Console.WriteLine("Stopping Nancy");
            host.Stop();  // stop hosting
            Console.WriteLine("Closing uDMX");
            DMXControl.Instance.Dispose();

        }
    }
}
