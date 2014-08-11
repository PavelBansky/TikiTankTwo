using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class Glow : IEffect
    {
        public Glow()
        {
            rng = new Random();
            this.Color = Color.DarkGreen;
        }

        public void Activate(Color[] pixels)
        {
            
        }

        public void Deactivate(Color[] pixels)
        {
           
        }

        public int Update(Color[] pixels)
        {            
            Color current = Color.FromArgb(
                Color.R - rng.Next(0, Color.R / 16 + 1),
                Color.G - rng.Next(0, Color.G / 16 + 1),
                Color.B - rng.Next(0, Color.B / 16 + 1)
                );
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = current;

            return 0;
        }

        public string Argument
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        private Random rng;
    }
}
