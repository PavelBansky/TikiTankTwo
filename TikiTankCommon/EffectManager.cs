using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TikiTankCommon
{
    public class EffectManager
    {
        private const int THREAD_JOIN_WAIT = 2000;
        public EffectManager()
        {
            _effectList = new List<Effect>();
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
            int delay = 0;
            DateTime startTime = DateTime.Now;

            while (_isRunning)
            {
                TimeSpan delta = DateTime.Now - startTime;
                if (delta.TotalMilliseconds >= delay)
                {
                    lock (this)
                    {
                        delay = ActiveEffect.Step();
                    }
                    startTime = DateTime.Now;
                }
            }
            Console.WriteLine("Exiting thread");
        }

        public Effect ActiveEffect
        {
            get { return _effectList[_activeIndex]; }
        }

        private bool _isRunning = false;
        private Thread _thread;
        private int _activeIndex;
        private List<Effect> _effectList;
    }
}
