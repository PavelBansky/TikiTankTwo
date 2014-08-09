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
            _speed = 0;
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
            byte rawSpeed;

            while (_isRunning)
            {                
                // If there is something to read, read it
                if (_port.BytesToRead > 0)
                {
                    // read and discard the rest of the buffer
                    rawSpeed = (byte)_port.ReadByte();
                    _port.DiscardInBuffer();                 
                                       
                    // if speed changed, act
                    if (rawSpeed != _speed)
                    {                        
                        if (OnSpeedChanged != null)
                            OnSpeedChanged(_speed, rawSpeed);

                        lock (this)
                        {
                            _speed = rawSpeed;
                        }
                    }
                }
            }

            _port.Close();
        }

        public delegate void SpeedChanged(byte oldSpeed, byte changedSpeed);
        public event SpeedChanged OnSpeedChanged;

        public byte Speed
        {
            get { return _speed; }
        }

        byte _speed;
        SerialPort _port;
        Thread _thread;
        bool _isRunning;
    }
}
