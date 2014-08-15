using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace TikiTankHardware
{
    public class SpeedSensor
    {
        public SpeedSensor(string portName)
        {
            _port = new SerialPort(portName, 9600);  
        }

        public void Start()
        {
            Console.WriteLine("Sensor: Starting Speed Sensor");
            _isRunning = true;
            _thread = new Thread(DoWork);
            _thread.Start();
        }

        public void Stop()
        {
            Console.WriteLine("Sensor: Stoping Speed Sensor");
            _isRunning = false;
            _thread.Join(2000);            
        }

        public void DoWork()
        {
            _port.Open();

            byte[] buffer = new byte[256];
            int ticksNumber;

            while (_isRunning)
            {                
                // If there is something to read, read it
                if (_port.BytesToRead > 0)
                {
                    // read and discard the rest of the buffer
                    ticksNumber = _port.ReadByte();
                    Console.WriteLine("Tick: count {0}", ticksNumber);

                    if (OnSpeedChanged != null)
                    {                            
                        for (; ticksNumber > 0; ticksNumber--)
                        {                            
                            OnSpeedChanged();
                        }
                    }
                }
            }
    
            _port.Close();
        }

        public delegate void SpeedChanged();
        public event SpeedChanged OnSpeedChanged;

        SerialPort _port;
        Thread _thread;
        bool _isRunning;
    }
}
