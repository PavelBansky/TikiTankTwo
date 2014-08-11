using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon
{
    public static class DisplayHelper
    {
        public static void FillColor(Color[] Pixels, int start, int count, Color color)
        {
            for (int i = start; i < start + count; i++)
            {
                Pixels[i] = color;
            }
        }

        public static void RotateRight(Color[] Pixels)
        {
            Color lastColor = Pixels[Pixels.Length - 1];
            ShiftRight(Pixels);
            Pixels[0] = lastColor;            
        }

        public static void RotateLeft(Color[] Pixels)
        {
            Color firstColor = Pixels[0];
            ShiftLeft(Pixels);
            Pixels[Pixels.Length - 1] = firstColor;
        }

        public static void ShiftRight(Color[] Pixels)
        {
            for (int i = Pixels.Length - 1; i > 0; i--)
            {
                Pixels[i] = Pixels[i - 1];
            }

            Pixels[0] = Color.Black;
        }

        public static void ShiftLeft(Color[] Pixels)
        {
            for (int i = 0; i < Pixels.Length - 1; i++)
            {
                Pixels[i] = Pixels[i + 1];
            }

            Pixels[Pixels.Length - 1] = Color.Black;
        }

    }
}
