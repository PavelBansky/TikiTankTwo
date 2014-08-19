using System.Runtime.Serialization;
using System.Drawing;
using System;

namespace TikiTankCommon.Effects
{
    public class TwinkleTwinkle : IEffect
    {
        public TwinkleTwinkle()
        {
            DecayRate = 50;
            CycleRate = 500;
            CreateRate = 500; 
            rng = new Random();
        }


        public void Activate(System.Drawing.Color[] pixels)
        {
            lastCycle = lastDecay = lastCreate = DateTime.Now;

            indices = new int[pixels.Length];
            memory = new Color[pixels.Length];
        }

        public void Deactivate(System.Drawing.Color[] pixels) { }

        public bool WouldUpdate()
        {
            // stabilize the image by only drawing every other frame
            //		return DateTime.Now > lastDecay + TimeSpan.FromMilliseconds(DecayRate);
            TimeSpan since = DateTime.Now - lastDecay;
            return (since > TimeSpan.FromMilliseconds(DecayRate));
        }

        public void FrameUpdate(Color[] pixels)
        {
            while (lastCycle < DateTime.Now)
            {
                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] != 0)
                        indices[i]++;
                    if (indices[i] >= Palette.Length)
                        indices[i] = 0;
                }

                lastCycle += TimeSpan.FromMilliseconds(CycleRate);
            }

            while(lastDecay < DateTime.Now)
            {
                lastDecay += TimeSpan.FromMilliseconds(DecayRate);

                for (int i = 0; i < indices.Length; i++)
                {
                    pixels[i] = Color.FromArgb(
                            (byte)TwinkleTwinkle.Palette[indices[i]].R,
                            (byte)TwinkleTwinkle.Palette[indices[i]].G,
                            (byte)TwinkleTwinkle.Palette[indices[i]].B 
                            );
                }
            }

            while (lastCreate < DateTime.Now)
            {
                lastCreate += TimeSpan.FromMilliseconds(CreateRate);
                int index = rng.Next(pixels.Length);
                indices[index] = 1;
                pixels[index] = Color.White;
            }
        }

        private static Color[] Palette = { Color.Black, Color.White, Color.LavenderBlush, Color.MintCream, 
                    Color.Coral, Color.LightGray, Color.Aquamarine, Color.LightSkyBlue, Color.Silver,
                    Color.LightGray, Color.Black };

        public void Tick() { }
        public bool IsSensorDriven { get; set; }
        public string Argument
        {
            get { return _arg.ToString();  }
            set 
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    if (i > 0)
                    {
                        _arg = i;
                        CreateRate = 1000 / i;
                    }
                }
            }
        }
        public System.Drawing.Color Color { get; set; }

        public int CreateRate { get; set; }
        public int DecayRate { get; set; }
        public int CycleRate { get; set; }

        private int _arg;
        private Color[] memory;
        private int[] indices;
        private Random rng;
        private DateTime lastDecay, lastCycle, lastCreate;
    }

}