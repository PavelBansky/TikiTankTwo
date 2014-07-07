using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankCommon
{
    public class EffectManager
    {
        public EffectManager()
        {
            _effectList = new List<Effect>();
        }

        public void AddEffect(Effect effect)
        {
            _effectList.Add(effect);
        }

        public void SelectEffect(int index)
        {
            if (index < _effectList.Count)
            {
                _effectList[_activeIndex].Deactivate();
                _activeIndex = index;
                _effectList[_activeIndex].Activate();
            }
        }

        public Effect ActiveEffect
        {
            get { return _effectList[_activeIndex]; }
        }

        private int _activeIndex;
        private List<Effect> _effectList;
    }
}
