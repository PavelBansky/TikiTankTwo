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
            _effectList = new List<Effect>();
            Speed = 0;            
        }

        public void AddEffect(Effect effect)
        {
            lock (this)
            {
                _effectList.Add(effect);
            }
        }

        public void SelectEffect(int index)
        {
            lock (this)
            {
                if (index < _effectList.Count)
                {                                        
                    _effectList[_activeIndex].Deactivate();
                    _activeIndex = index;                    
                    _effectList[_activeIndex].Activate();
                }
            }
        }

        public void SelectIdleEffect(int index)
        {
            if (index < _effectList.Count)
            {                
                _idleIndex = index;             
            }
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

        public List<EffectInformation> GetEffectsInformation()
        {
            List<EffectInformation> result = new List<EffectInformation>();
            for(int i=0; i<_effectList.Count; i++)
            {
                EffectInformation info = new EffectInformation(_effectList[i].Information);
                info.Id = i;
                result.Add(info);
            }

            return result;
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

            int delay = 0;
            startTime = DateTime.Now;
            TimeSpan delta;

            while (_isRunning)
            {
                delta = DateTime.Now - startTime;

                // If we are sensor driven
                if (ActiveEffect.IsSensorDriven)
                {                    
                    // And we are running and it's time to tick
                    if (State == TankState.Running && Speed > 0 && delta.TotalMilliseconds >= delay)
                    {
                        ActiveEffectStep();                         
                        delay = Speed;
                    }
                    // If we are sensor based on idle and it's time to tick
                    else if (State == TankState.Idle && delta.TotalMilliseconds >= delay)
                    {
                        delay = IdleEffectStep();                            
                    }
                }
                // If we are not sensor driven
                else
                {                    
                    // And it's time to tick
                    if (delta.TotalMilliseconds >= delay)
                    {
                        // do the step
                        delay = ActiveEffectStep();                        
                    }
                }
            }

            Console.WriteLine("Exiting thread");
        }

        // Thread-safe step
        private int ActiveEffectStep()
        {
            int delay;
            lock (this)
            {
                delay = ActiveEffect.Step();
                startTime = DateTime.Now;
            }

            return delay;
        }

        private int IdleEffectStep()
        {
            int delay;
            lock (this)
            {
                delay = IdleEffect.Step();
                startTime = DateTime.Now;
            }

            return delay;
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

        public Effect IdleEffect
        {
            get { return _effectList[_idleIndex]; }
        }

        public Effect ActiveEffect
        {
            get { return _effectList[_activeIndex]; }
        }

        public int Speed { get; set; }


        private DateTime startTime;
        private TankState _state;
        private bool _isRunning = false;
        private Thread _thread;
        private int _activeIndex, _idleIndex;
        private List<Effect> _effectList;
    }
}
