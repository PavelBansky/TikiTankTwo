using System;
using System.Timers;
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
            State = TankState.Running;

            idleTimer = new Timer(60000);
            idleTimer.Enabled = true;
            idleTimer.Elapsed += idleTimer_Elapsed;            
        }

        /// <summary>
        /// Event handler called when tank was idle for a while (minute)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void idleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Tank State: Idle");
            idleTimer.Enabled = false;
            State = TankState.Idle;
        }

        /// <summary>
        /// Event handler called when speed is changing
        /// </summary>
        /// <param name="oldSpeed"></param>
        /// <param name="changedSpeed"></param>
        static void _sensor_OnSpeedChanged()
        {

        }

        private static SpeedSensor _sensor;
        public static SpeedSensor Sensor
        {
            get { return _sensor; }
            set
            {
                _sensor = value;
                _sensor.OnSpeedChanged += _sensor_OnSpeedChanged;
            }
        }

        private static TankState _state;
        public static TankState State
        {
            get { return _state;  }
            set
            {
                _state = value;
                //_state = TreadsManager.State = BarrelManager.State = PanelsManager.State = value;
            }
        }

        private static Timer idleTimer = new Timer();

        public static EffectManager TreadsManager { get; set; }
        public static EffectManager BarrelManager { get; set; }
        public static EffectManager PanelsManager { get; set; }
    }
}
