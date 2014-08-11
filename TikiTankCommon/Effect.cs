using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace TikiTankCommon
{
    public interface IEffect
    {
        /// <summary>
        /// Method is called when effect is activated
        /// </summary>
        void Activate(Color[] pixels);
        /// <summary>
        /// Method is called when effect is being deactived
        /// </summary>
        void Deactivate(Color[] pixels);
        /// <summary>
        /// Perform one step of the effect
        /// </summary>
        /// <returns>Time for which is next step expected</returns>
        int Update(Color[] pixels);
        /// <summary>
        /// Gets or Sets the effect parameter
        /// </summary>
        string Argument { get; set; }
        /// <summary>
        /// Gets or Sets the effect color
        /// </summary>
        Color Color { get; set; }
    }
}
