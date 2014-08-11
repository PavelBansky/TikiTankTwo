using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon
{
    public class EffectContainer
    {
        public EffectContainer(IEffect effect, LEDStrip strip, EffectInfo info)
        {
            this.Information = info;
            this.Effect = effect;
            this.LedStrip = strip;
        }

        public void Activate()
        {
            Effect.Activate(LedStrip.Pixels);
            LedStrip.Show();
        }

        public void Deactivate()
        {
            Effect.Deactivate(LedStrip.Pixels);
            LedStrip.Show();
        }

        public int Update()
        {
            int result = Effect.Update(LedStrip.Pixels);
            LedStrip.Show();

            return result;
        }

        IEffect Effect;
        LEDStrip LedStrip;

        public string Argument 
        {
            get { return Effect.Argument;  }
            set { Effect.Argument = value; }
        }
        public Color Color 
        {
            get { return Effect.Color; }
            set { Effect.Color = value; }
        }

        public bool IsSensorDriven { get; set; }
        public EffectInfo Information { get; set; }
        
    }
}
