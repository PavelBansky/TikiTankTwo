using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TikiTankCommon;

namespace TikiTankServer.Managers
{
    public class EffectManager
    {
        private const int THREAD_JOIN_WAIT = 2000;
        public EffectManager()
        {
            _effectList = new List<EffectContainer>();
            Speed = 0;            
        }

        public void AddEffect(EffectContainer effect)
        {
            lock (this)
            {
                _effectList.Add(effect);
            }
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

        public void Start()
        {
            _isRunning = true;
            _thread = new Thread(DoWork);
            _thread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _thread.Join(THREAD_JOIN_WAIT);
        }

        private void DoWork()
        {
            Console.WriteLine("Starting thread");

            int tickDelay = 0;
            tickStartTime = DateTime.Now;
            DateTime refreshStartTime = DateTime.Now;
            TimeSpan tickDelta, refreshDelta;

            while (_isRunning)
            {
                tickDelta = DateTime.Now - tickStartTime;
                refreshDelta = DateTime.Now - refreshStartTime;

                // If we are sensor driven
                if (ActiveEffect.IsSensorDriven)
                {                    
                    // And we are running and it's time to tick
                    if (State == TankState.Running && Speed > 0 && tickDelta.TotalMilliseconds >= tickDelay)
                    {
                        ActiveEffectStep();                         
                        tickDelay = Speed;
                    }
                }

                // Update the Frame every 50 miliseconds
                if (refreshDelta.TotalMilliseconds >= 50)
                {
                    refreshStartTime = DateTime.Now;
                    EffectUpdate();
                }

            }

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

        public int Speed { get; set; }


        private DateTime tickStartTime;
        private TankState _state;
        private bool _isRunning = false;
        private Thread _thread;
        private int _activeIndex, _idleIndex;
        private List<EffectContainer> _effectList;
    }
}
