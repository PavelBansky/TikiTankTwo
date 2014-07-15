using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TikiTankCommon
{
    public abstract class Effect
    {
        /// <summary>
        /// Method is called when effect is activated
        /// </summary>
        public abstract void Activate();
        /// <summary>
        /// Method is called when effect is being deactived
        /// </summary>
        public abstract void Deactivate();
        /// <summary>
        /// Perform one step of the effect
        /// </summary>
        /// <returns>Time for which is next step expected</returns>
        public abstract int Step();
        /// <summary>
        /// Gets or Sets the effect parameter
        /// </summary>
        public abstract string Argument { get; set; }
        /// <summary>
        /// Gets or Sets the effect color
        /// </summary>
        public abstract Color Color { get; set; }
        public bool SensorDriven { get; set; }
        public EffectInfo Information { get; set; }
        protected LEDStrip LedStrip { get; set; }
    }
}
