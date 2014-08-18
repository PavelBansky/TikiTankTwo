using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class Rainbow : IEffect
    {
        public Rainbow()
        {
            this.Argument = "8";
            offset = 0;
            startTime = DateTime.Now;
        }

        public void Activate(Color[] pixels) { }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
            TimeSpan delta = DateTime.Now - startTime;
            /*            if (delta.TotalMilliseconds > _delay)
                        {
                            startTime = DateTime.Now;
                            return true;
                        }
                        return false; 
            */
            return true;
        }

        public void FrameUpdate(Color[] pixels)
        {
            offset = (offset + 1) % pixels.Length;
            for (int i = 0; i < pixels.Length; i++)
            {
                // replace with Pavel's rotateRight instead of this offset nonsense
                int position = ((pixels.Length - 1) - i - offset) % pixels.Length;
                // (30 / 2 Pi) * position
                // one divided by period of sine wave is unit times 30 positions
                int redWave = (int)(Math.Sin(20 * position / Math.PI));
                // rotate 1/3 of the way around the circle
                // (30 / 2 Pi) * (position + (pixels.Length / 3))
                int greenWave = (int)(Math.Sin(15 * position + (10 + pixels.Length / 3) / Math.PI));
                // rotate 2/3 of the way around the circle
                int blueWave = (int)(Math.Sin(15 * position + (2 * pixels.Length / 3) / Math.PI));
                //Console.WriteLine("r: {0}, g: {0}, b: {0}");
                // apply sines to colors
                pixels[position] = Color.FromArgb((int)(redWave * 255), (int)(greenWave * 255), (int)(blueWave * 255));
            }

            // cycle through all position start points
            offset = (offset < pixels.Length) ? offset + 1 : 0;
        }

        public void Tick()
        {

        }

        public bool IsSensorDriven { get; set; }
        public string Argument
        {
            get
            {
                return _arg.ToString();
            }
            set
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    if (i > 0)
                    {
                        _arg = Math.Abs(i);
                        _delay = 400 / _arg;
                    }
                }
            }
        }

        public Color Color { get; set; }
        
        private int _delay;
        private int _arg;
        private DateTime startTime;
        private int offset;

    }
}
