using System;

namespace TikiTankCommon
{
    public interface IEffect
    {
        /// <summary>
        /// Method is called when effect is activated
        /// </summary>
        void Activate(System.Drawing.Color[] pixels);
        /// <summary>
        /// Method is called when effect is being deactived
        /// </summary>
        void Deactivate(System.Drawing.Color[] pixels);
        /// <summary>
        /// Perform one step of the effect
        /// </summary>
        /// <returns>Time for which is next step expected</returns>
        int Update(System.Drawing.Color[] pixels);
        /// <summary>
        /// Gets or Sets the effect parameter
        /// </summary>
        string Argument { get; set; }
        /// <summary>
        /// Gets or Sets the effect color
        /// </summary>
        System.Drawing.Color Color { get; set; }
    }
}
