using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace TikiTankHardware
{
	public class SpeedSensor
	{
		byte[] recvBuffer = new byte[1];
		Stream stream;
		SerialPort port;

		public SpeedSensor(string portName)
		{
			port = new SerialPort(portName, 9600);
		}

		public void Start()
		{
			Console.WriteLine("Sensor: Starting Speed Sensor");

			port.Open();
			stream = port.BaseStream;

			// C# serial port class is dangerous, only the BaseStream is safe
			// http://www.sparxeng.com/blog/software/must-use-net-system-io-ports-serialport
			stream.BeginRead(recvBuffer, 0, recvBuffer.Length, OnRecv, null);
		}

		private void OnRecv(IAsyncResult ar)
		{
			try
			{
				var len = stream.EndRead(ar);

				for (var i = 0; i < len; ++i)
					for (var j = 0; j < recvBuffer[i]; ++j)
						OnTick();

				stream.BeginRead(recvBuffer, 0, recvBuffer.Length, OnRecv, null);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Sensor: {0}", ex.Message);
			}
		}

		public void Stop()
		{
			if (stream != null)
			{
				stream.Close();
				stream = null;
			}
		}

		public delegate void TickDelegate();
		public event TickDelegate OnTick;

	}
}
