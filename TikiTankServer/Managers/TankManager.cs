using System;
using System.Timers;
using TikiTankHardware;
using TikiTankServer.Managers;

namespace TikiTankServer.Managers
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

            SchedulerManager = new SchedulerManager();
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
        static void _sensor_OnSpeedChanged(byte oldSpeed, byte changedSpeed)
        {
            //TreadsManager.Speed = BarrelManager.Speed = SidesManager.Speed = changedSpeed;

            Console.WriteLine("Tank State: Speed changed from {0} to {1}", oldSpeed, changedSpeed);

            // If are moving keep the idleTimer off
            if (changedSpeed > 0)
            {
                idleTimer.Enabled = false;
                State = TankState.Running;
            }
            // otherwise start the idle timer
            else
            {                
                idleTimer.Enabled = true;
            }
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
                //_state = TreadsManager.State = BarrelManager.State = SidesManager.State = value;
                _state = value;
            }
        }

        private static Timer idleTimer = new Timer();

        
        public static SchedulerManager SchedulerManager { get; set; }
        /*
        public static EffectManager BarrelManager { get; set; }
        public static EffectManager SidesManager { get; set; }
         */
    }
}
