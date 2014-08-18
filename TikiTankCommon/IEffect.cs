﻿using System;

namespace TikiTankCommon
{
    public interface IBarrelEffect : IEffect
    {
    }

    public interface IPanelEffect : IEffect
    {
    }

    public interface ITreadEffect : IEffect
    {
    }

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
        void FrameUpdate(System.Drawing.Color[] pixels);

        bool WouldUpdate();

        void Tick();

        bool IsSensorDriven { get; set; }
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
