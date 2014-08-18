using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class SpotTread : IEffect
    {
        enum Direction
        {
            Stop,
            Forward,
            Backward
        }

        public SpotTread()
        {
            this.Argument = "0";
            this.Color = Color.FromArgb(255, 255, 255);
            startTime = DateTime.Now;
        }

        public void Activate(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[(pixels.Length - 1) - i] = this.Color;
            }

            for (int j = 0; j < pixels.Length; j += 15)
            {
                StripHelper.FillColor(pixels, j + 5, 10, Color.Black);
            }

            memory = new Color[pixels.Length];
            Array.Copy(pixels, memory, pixels.Length);
        }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
            return true;
        }

        public void FrameUpdate(Color[] pixels)
        {
            if (!IsSensorDriven)
            {
                TimeSpan delta = DateTime.Now - startTime;
                if (delta.TotalMilliseconds > _delay)
                {
                    startTime = DateTime.Now;
                    Tick();
                }
            }

            Array.Copy(memory, pixels, pixels.Length);
        }

        public void Tick()
        {
            if (_direction == Direction.Forward)
                StripHelper.RotateRight(memory);
            else if (_direction == Direction.Backward)
                StripHelper.RotateLeft(memory);
        }

        public bool IsSensorDriven { get; set; }

        public Color Color { get; set; }

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
                    _arg = i;

                    if (i < 0)
                        _direction = Direction.Backward;
                    else if (i > 0)
                        _direction = Direction.Forward;
                    else if (i == 0)
                        _direction = Direction.Stop;

                    if (Math.Abs(i) > 0)
                        _delay = 400 / Math.Abs(i);
                }
            }
        }

        private int _delay;
        private int _arg;
        private Direction _direction;
        private Color[] memory;
        private DateTime startTime;
    }
}
