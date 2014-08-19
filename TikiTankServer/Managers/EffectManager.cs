using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TikiTankCommon;
using TikiTankHardware;

namespace TikiTankServer.Managers
{
    public class EffectManager
    {
		private const int FRAME_DELAY_USEC = 15000;
        private const int THREAD_JOIN_WAIT = 2000;
                
        public EffectManager(SpeedSensor speedSensor)
        {
            _effectList = new List<EffectContainer>();
            sensor = speedSensor;
            sensor.OnTick += sensor_OnTick;
        }

        public void AddEffect(EffectContainer cont)
        {
            _effectList.Add(cont);
        }

        
        void sensor_OnTick()
        {
            if (ActiveEffect != null && ActiveEffect.IsSensorDriven)
                ActiveEffectStep();
        }

        public EffectData SelectEffect(int index)
        {
            EffectData result = new EffectData(new EffectInfo());
            lock (this)
            {
                if (index >= 0 && index < _effectList.Count)
                {                                        
                    _effectList[_activeIndex].Deactivate();
                    _activeIndex = index;
                    result = GetEffectData(index);

                    _effectList[_activeIndex].Activate();
                }
            }

            return result;
        }

        public EffectData GetActiveEffectData()
        {
            EffectData result = new EffectData(new EffectInfo());
            lock (this)
            {
                result = GetEffectData(_activeIndex);
            }

            return result;
        }

        public List<EffectData> GetEffectsList()
        {
            List<EffectData> result = new List<EffectData>();
            for (int i = 0; i < _effectList.Count; i++)
            {
                EffectData info = new EffectData(_effectList[i].Information);
                info.Id = i;
                result.Add(info);
            }

            return result;
        }

        public void SetSensorDrive(bool status)
        {
            _effectList[_activeIndex].IsSensorDriven = status;
            _effectList[_activeIndex].Activate();
        }

        public void SelectIdleEffect(int index)
        {
            if (index < _effectList.Count)
            {                
                _idleIndex = index;             
            }
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _thread = new Thread(DoWork);
                _thread.Start();
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _thread.Join(THREAD_JOIN_WAIT);
        }

        [DllImport("libc")]
        private static extern void usleep(int time);

        private void DoWork()
        {
            Console.WriteLine("Starting thread");

            tickStartTime = DateTime.Now;

            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            var next = sw.ElapsedTicks;
            var ticksPerUsec = System.Diagnostics.Stopwatch.Frequency / 1000000;

            while (_isRunning)
            {
                next += FRAME_DELAY_USEC;

                EffectUpdate();

                var wait = (int)Math.Max(0, next - (sw.ElapsedTicks / ticksPerUsec));

                usleep(wait);
            }

            sw.Stop();

            Console.WriteLine("Exiting thread");
        }

        // Thread-safe step
        private void ActiveEffectStep()
        {
            lock (this)
            {
                ActiveEffect.Tick();
                tickStartTime = DateTime.Now;
            }            
        }
        
        private void EffectUpdate()
        {
            lock (this)
            {
                if (ActiveEffect.IsSensorDriven && State == TankState.Idle)
                    IdleEffect.Update();
                else
                    ActiveEffect.Update();
            }
        }

        /// <summary>
        /// Gets status data about affect of a given index.
        /// This call is thread UN-SAFE!!
        /// </summary>
        /// <param name="index"></param>
        /// <returns>EffectData</returns>
        private EffectData GetEffectData(int index)
        {
            EffectData result = new EffectData(new EffectInfo());

            if (index >= 0 && index < _effectList.Count)
            {
                result = new EffectData(_effectList[index].Information);
                result.Id = index;
                result.Color = ColorHelper.ColorToString(_effectList[index].Color);
                result.Argument = (_effectList[index].Argument != null) ? _effectList[index].Argument : string.Empty;
            }

            return result;
        }

        private void ActivateRunningEffect()
        {
            lock (this)
            {
                _effectList[_idleIndex].Deactivate();
                _effectList[_activeIndex].Activate();

            }
        }

        private void ActivateIdleEffect()
        {
            lock (this)
            {
                _effectList[_activeIndex].Deactivate();
                _effectList[_idleIndex].Activate();
            }
        }

        public TankState State 
        {
            get { return _state; }
            set
            {
                // Transition from running to idle when in sensor driven mode
                if (_state == TankState.Running && value == TankState.Idle && ActiveEffect.IsSensorDriven)
                {
                    ActivateIdleEffect();
                }
                // Transitioin from idle to running when in sensor driven mode
                else if (_state == TankState.Idle && value == TankState.Running && ActiveEffect.IsSensorDriven)
                {
                    ActivateRunningEffect();
                }

                _state = value;
            }
        }

        public EffectContainer IdleEffect
        {
            get { return _effectList[_idleIndex]; }
        }

        public EffectContainer ActiveEffect
        {
            get { return _effectList[_activeIndex]; }
        }

        private List<EffectContainer> _effectList;
        public  List<EffectContainer> Effects
        {
            get 
            {
                if (_isRunning)
                    throw new Exception("Crash Boom Bang: you are trying to access the effects list while the tank is running");

                return _effectList; 
            }
        }

        private SpeedSensor sensor;
        private DateTime tickStartTime;
        private TankState _state;
        private bool _isRunning = false;
        private Thread _thread;
        private int _activeIndex, _idleIndex;
        
    }
}
