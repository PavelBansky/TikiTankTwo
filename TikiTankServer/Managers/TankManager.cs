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

            changeIdleTimer = new Timer(5000);
            changeIdleTimer.Enabled = false;
            changeIdleTimer.Elapsed += changeIdleTimer_Elapsed;

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
            TankManager.TreadsManager.NextIdleEffect();            
            TankManager.TreadsManager.Start();

            Console.WriteLine("Barrel Manager: ");
            BarrelManager = new EffectManager(Sensor, manualTickTimer);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/barrel.json"), BarrelLED, BarrelManager.Effects);
            Console.WriteLine("Effects loaded {0}", BarrelManager.Effects.Count);
            TankManager.BarrelManager.SelectEffect(0);
            TankManager.BarrelManager.NextIdleEffect();
            TankManager.BarrelManager.Start();

            Console.WriteLine("Panel Manager: ");
            PanelsManager = new EffectManager(Sensor, manualTickTimer);
            SettingsLoader.LoadEffects(Path.Combine(basePath,"Settings/panels.json"), DmxLED, PanelsManager.Effects);
            Console.WriteLine("Effects loaded {0}", PanelsManager.Effects.Count);
            TankManager.PanelsManager.SelectEffect(0);
            TankManager.PanelsManager.NextIdleEffect();
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

        public static void SetScreenSaverInterval(double interval)
        {
            if (interval > 0)
            {
                changeIdleTimer.Interval = interval*1000;                
            }           
        }

        public static SettingsData GetSettings()
        {
            SettingsData result = new SettingsData();
            result.DmxBrightness = DmxControl.Brightness;
            result.IdleInterval = (changeIdleTimer.Interval / 1000);
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

        private static Timer idleTimer = new Timer();
        private static Timer changeIdleTimer = new Timer();
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
