using System.Drawing;

namespace TikiTankCommon
{
    public class EffectContainer
    {
        public EffectContainer()
        {

        }

        public EffectContainer(IEffect effect, LEDStrip strip, EffectInfo info)
        {
            this.Information = info;
            this.Effect = effect;
            this.LedStrip = strip;
        }

        public void AssignStrip(LEDStrip strip)
        {
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

        public void Update()
        {
            if (Effect.WouldUpdate())
            {
                Effect.FrameUpdate(LedStrip.Pixels);
            }

            LedStrip.Show();
        }

        public void Tick()
        {
            Effect.Tick();
        }

        public IEffect Effect { get; set; }

        private LEDStrip LedStrip;

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

        public bool IsScreenSaver
        {
            get { return Information.IsScreenSaver; }
            set { Information.IsScreenSaver = value; }
        }

        public bool IsSensorDriven 
        {
            get { return Effect.IsSensorDriven; }
            set { Effect.IsSensorDriven = value;  }
        }
        public EffectInfo Information { get; set; }
        
    }
}
