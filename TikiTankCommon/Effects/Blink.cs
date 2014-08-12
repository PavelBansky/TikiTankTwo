using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class Blink : IEffect
    {
        public Blink()
        {
            Color = Color.Red;
            darkFrame = false;
        }

        public void Activate(System.Drawing.Color[] pixels)
        {
            // No need for first frame
        }

        public void Deactivate(System.Drawing.Color[] pixels)
        {
            // No need for last frame
        }

        public int Update(System.Drawing.Color[] pixels)
        {
            // Use current user select color
            Color activeColor = Color;

            // Unless we are in the frame that needs to black
            if (darkFrame)
                activeColor = Color.Black;

            // Fill the strip with color
            for (int i = 0; i < pixels.Length; i++ )
            {
                pixels[i] = activeColor;
            }

            // Switch for the next frame
            darkFrame = !darkFrame;

            // Request to be called again in 500 milliseconds
            return 500;
        }

        public string Argument { get; set; }
        public Color Color { get; set; }

        bool darkFrame;
    }
}
