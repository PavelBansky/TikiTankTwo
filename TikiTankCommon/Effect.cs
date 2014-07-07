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
        /// Set effect parameter
        /// </summary>
        /// <param name="argument">Parameter</param>
        public abstract void SetArgument(string argument);
        /// <summary>
        /// Get effect parameter
        /// </summary>
        /// <returns>Effect parameter</returns>
        public abstract string GetArgument();

        public abstract void SetColor(Color color);
        public abstract Color GetColor();
        public bool SensorDriven { get; set; }
        public EffectInfo Information { get; set; }
        protected LEDStrip LedStrip { get; set; }
    }
}
