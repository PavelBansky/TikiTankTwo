using System;
using System.IO;
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

    public static class TankManager
    {       
        static TankManager()
        {
            idleTimer = new Timer(60000);
            idleTimer.Enabled = false;
            idleTimer.Elapsed += idleTimer_Elapsed;

            manualTickTimer = new Timer();
            manualTickTimer.Enabled = false;                        
        }

        public static void StartTheTank()
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;

            Console.WriteLine("Tread Manager: ");
            TreadsManager = new EffectManager(Sensor, manualTickTimer);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/treads.json"), TreadsLED, TreadsManager.Effects);
            Console.WriteLine("Effects loaded {0}", TreadsManager.Effects.Count);
            TankManager.TreadsManager.SelectEffect(0);
            TankManager.TreadsManager.SelectIdleEffect(1);            
            TankManager.TreadsManager.Start();

            Console.WriteLine("Barrel Manager: ");
            BarrelManager = new EffectManager(Sensor, manualTickTimer);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/barrel.json"), BarrelLED, BarrelManager.Effects);
            Console.WriteLine("Effects loaded {0}", BarrelManager.Effects.Count);
            TankManager.BarrelManager.SelectEffect(0);
            TankManager.BarrelManager.SelectIdleEffect(1);
            TankManager.BarrelManager.Start();

            Console.WriteLine("Panel Manager: ");
            PanelsManager = new EffectManager(Sensor, manualTickTimer);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/panels.json"), DmxLED, PanelsManager.Effects);
            Console.WriteLine("Effects loaded {0}", PanelsManager.Effects.Count);
            TankManager.PanelsManager.SelectEffect(0);
            TankManager.PanelsManager.SelectIdleEffect(1);
            TankManager.PanelsManager.Start();

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
                manualTickTimer.Interval = interval;
                manualTickTimer.Enabled = true;                
            }
            else
            {
                manualTickTimer.Enabled = false;
            }
        }

        public static double GetManualTickInterval()
        {
            return manualTickTimer.Interval;
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
            }
        }

        static void _sensor_OnTick()
        {
            if (State == TankState.Idle)
            {
                Console.WriteLine("Tank State: Running");
                State = TankState.Running;
                idleTimer.Enabled = true;
            }

            lastTick = DateTime.Now;
        }

        private static System.Timers.Timer manualTickTimer;

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

        private static Timer idleTimer = new Timer();

        public static EffectManager TreadsManager { get; set; }
        public static EffectManager BarrelManager { get; set; }
        public static EffectManager PanelsManager { get; set; }
        public static DMXControl DmxControl { get; set; }
        public static LEDStrip TreadsLED { get; set; }
        public static LEDStrip BarrelLED { get; set; }
        public static LEDStrip DmxLED { get; set; }
    }
}
