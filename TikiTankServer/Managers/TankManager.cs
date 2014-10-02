using System;
using System.IO;
using System.Threading;
using System.Timers;
using TikiTankCommon;
using TikiTankHardware;
using TikiTankServer.Managers;

namespace TikiTankServer
{
    public enum TankState
    {
        Running,
        Idle,
    }

	public class Timer
	{
		Thread thread;

		public event System.Timers.ElapsedEventHandler Elapsed;

		public Timer(int milliseconds)
		{
			Interval = milliseconds;
		}

		public int Interval { get; set; }

		public bool Enabled
		{
			get
			{
				return thread != null;
			}
			set
			{
				if (thread == null && value)
				{
					thread = new Thread(DoWork);
					thread.Start();
				}
				else if (thread != null && !value)
				{
					thread.Abort();
					thread = null;
				}
			}
		}

        [System.Runtime.InteropServices.DllImport("libc")]
        private static extern void usleep(int time);

        private void DoWork()
        {
            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            var next = sw.ElapsedTicks;
            var ticksPerUsec = System.Diagnostics.Stopwatch.Frequency / 1000000;

            while (true)
            {
                next += (Interval * 1000);

                if (Elapsed != null)
                    Elapsed(null, null);

                var wait = (int)Math.Max(0, next - (sw.ElapsedTicks / ticksPerUsec));

                usleep(wait);
            }
        }
	}

    public static class TankManager
    {       
        static TankManager()
        {
            idleTimer = new System.Timers.Timer(60000);
            idleTimer.Enabled = false;
            idleTimer.Elapsed += idleTimer_Elapsed;

            changeIdleTimer = new System.Timers.Timer(5*60000);
            changeIdleTimer.Enabled = false;
            changeIdleTimer.Elapsed += changeIdleTimer_Elapsed;

            manualTickTimer = new Timer(150);
            manualTickTimer.Enabled = false;
            manualTickTimer.Elapsed += manualTickTimer_Elapsed;
        }

		static void manualTickTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var effect = PanelsManager.ActiveEffect;
			if (effect != null)
				effect.Tick();

			effect = BarrelManager.ActiveEffect;
			if (effect != null)
				effect.Tick();

			effect = TreadsManager.ActiveEffect;
			if (effect != null)
				effect.Tick();
		}

        public static void StartTheTank()
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;

            Console.WriteLine("Tread Manager: ");
            TreadsManager = new EffectManager(Sensor);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/treads.json"), TreadsLED, TreadsManager.Effects);
            Console.WriteLine("Effects loaded {0}", TreadsManager.Effects.Count);
            TankManager.TreadsManager.SelectEffect(0);
            TankManager.TreadsManager.NextIdleEffect();            
            TankManager.TreadsManager.Start(15000, ThreadPriority.Highest);
            Thread.Sleep(5);

            Console.WriteLine("Barrel Manager: ");
            BarrelManager = new EffectManager(Sensor);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/barrel.json"), BarrelLED, BarrelManager.Effects);
            Console.WriteLine("Effects loaded {0}", BarrelManager.Effects.Count);
            TankManager.BarrelManager.SelectEffect(0);
            TankManager.BarrelManager.NextIdleEffect();
            TankManager.BarrelManager.Start(15000, ThreadPriority.Normal);
            Thread.Sleep(5);

            Console.WriteLine("Panel Manager: ");
            PanelsManager = new EffectManager(Sensor);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/panels.json"), DmxLED, PanelsManager.Effects);
            Console.WriteLine("Effects loaded {0}", PanelsManager.Effects.Count);
            TankManager.PanelsManager.SelectEffect(0);
            TankManager.PanelsManager.NextIdleEffect();
            TankManager.PanelsManager.Start(200000, ThreadPriority.Lowest);

            Sensor.Start();

            idleTimer.Enabled = true;
        }

        public static void StopTheTank()
        {
            TankManager.Sensor.Stop();

            Console.Write("Tread Manager: ");
            TreadsManager.Stop();
            Console.Write("Barrel Manager: ");
            BarrelManager.Stop();
            Console.Write("Sides Manager: ");
            PanelsManager.Stop();

            Console.WriteLine("Closing uDMX");
            DmxControl.Dispose(); 
        }

        public static void SetManualTick(double interval)
        {
            if (interval > 0)
            {
                manualTickTimer.Interval = Convert.ToInt32(interval);
                manualTickTimer.Enabled = true;
            }
            else
            {
                manualTickTimer.Enabled = false;
            }
        }

        public static void SetScreenSaverInterval(double interval)
        {
            if (interval > 0)
            {
                changeIdleTimer.Interval = interval*60000;             
            }           
        }

        public static SettingsData GetSettings()
        {
            SettingsData result = new SettingsData();
            result.DmxBrightness = DmxControl.Brightness;
            result.IdleInterval = (changeIdleTimer.Interval / 60000);
            result.ManualTick = manualTickTimer.Interval;

            return result;
        }



        /// <summary>
        /// Event handler called when tank was idle for a while (minute)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void idleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - lastTick).TotalMinutes > 1)
            {
                Console.WriteLine("Tank State: Idle");
                State = TankState.Idle;
                idleTimer.Enabled = false;
                changeIdleTimer.Enabled = true;
            }
        }

        static void changeIdleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {            
            TreadsManager.NextIdleEffect();
            BarrelManager.NextIdleEffect();
            PanelsManager.NextIdleEffect();
        }

        static void _sensor_OnTick()
        {
            if (State == TankState.Idle)
            {
                Console.WriteLine("Tank State: Running");
                State = TankState.Running;
                idleTimer.Enabled = true;
                changeIdleTimer.Enabled = false;
            }

            lastTick = DateTime.Now;
        }
      

        private static SpeedSensor _sensor;
        public static SpeedSensor Sensor
        {
            get { return _sensor; }
            set
            {
                _sensor = value;
                _sensor.OnTick += _sensor_OnTick;
            }
        }
        
        private static TankState _state;
        public static TankState State
        {
            get { return _state;  }
            set
            {
                _state = value;
                // I don't like this line, but I keep it for a bit
                _state = TreadsManager.State = BarrelManager.State = PanelsManager.State = value;
            }
        }

        private static DateTime lastTick;

        private static System.Timers.Timer idleTimer; 
        private static System.Timers.Timer changeIdleTimer; 
        private static Timer manualTickTimer;

        public static EffectManager TreadsManager { get; set; }
        public static EffectManager BarrelManager { get; set; }
        public static EffectManager PanelsManager { get; set; }
        public static DMXControl DmxControl { get; set; }
        public static LEDStrip TreadsLED { get; set; }
        public static LEDStrip BarrelLED { get; set; }
        public static LEDStrip DmxLED { get; set; }
    }
}
