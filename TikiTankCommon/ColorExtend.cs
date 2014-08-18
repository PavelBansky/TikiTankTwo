using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TikiTankCommon
{
    /// <summary>
    /// Some extention methods to make colors easier
    /// </summary>
    public static class ColorExtend
    {
        /// <summary>
        /// Floating point Lerp calculation. Usefull for graphics programming.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static float Lerp( this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

        /// <summary>
        /// Create a color an amount closer to another color. This can be used to 
        /// make a color darker, lighter to closer to another color.
        /// </summary>
        /// <remarks>
        /// <b>make red 50% lighter:</b>
        /// <para>
        /// Color.Red.Lerp( Color.White, 0.5f );
        /// </para>
        /// <b>make red 50% darker:</b>
        /// <para>
        /// Color.Red.Lerp( Color.Black, 0.5f );
        /// </para>
        /// <b>make white 10% bluer:</b>
        /// <para>
        /// Color.White.Lerp( Color.Blue, 0.1f );
        /// </para>
        /// </remarks>
        /// <param name="colour"></param>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Color Lerp(this Color colour, Color to, float amount)
        {
            // start colours as lerp-able floats
            float sr = colour.R, sg = colour.G, sb = colour.B;

            // end colours as lerp-able floats
            float er = to.R, eg = to.G, eb = to.B;

            // lerp the colours to get the difference
            byte r = (byte) sr.Lerp(er, amount),
                 g = (byte) sg.Lerp(eg, amount),
                 b = (byte) sb.Lerp(eb, amount);

            // return the new colour
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Make a color brighter by an amount.
        /// </summary>
        /// <param name="colour">Color to change</param>
        /// <param name="amount">Amount to change (0.5f == 50%)</param>
        /// <returns>Returns adjusted color</returns>
        public static Color MakeDarker(this Color colour, float amount)
        {
            return colour.Lerp(Color.Black, amount);
        }


        /// <summary>
        /// Make a color darker by an amount
        /// </summary>
        /// <param name="colour">Color to change</param>
        /// <param name="amount">Amount to change (0.5f == 50%)</param>
        /// <returns></returns>
        public static Color MakeBrighter(this Color colour, float amount)
        {
            return colour.Lerp(Color.White, amount);
        }
    }
}
