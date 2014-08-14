using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace TikiTankCommon.Effects
{    
    public class SimpleTread : IEffect
    {
        enum Direction
        {            
            Stop,
            Forward,
            Backward            
        }

        public SimpleTread(bool rainbowColors)
        {         
            this.Argument ="0";
            this.Color = Color.FromArgb(255, 255, 255);
            this.rainbowEnabled = rainbowColors;
            this.startTime = DateTime.Now;                                
        }

        public bool WouldUpdate()
        {
            return true;
        }

        public void Activate(Color[] pixels)
        {            
            startIndex = 0;
            memory = new Color[15];
        }

        public void Deactivate(Color[] pixels) { }

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

            pixelColor = (rainbowEnabled) ? ColorHelper.Wheel(((counter++ * 384 / 90)) % 384) : this.Color;
            if (counter >= pixels.Length)
                counter = 0;


            for (int j = startIndex; j < pixels.Length-startIndex; j += 15)
            {
                int count = ((pixels.Length - j) < 5) ? (pixels.Length - j) : 5;         
                StripHelper.FillColor(pixels, j, count, pixelColor);

                count = ((pixels.Length - (j+5)) < 10) ? (pixels.Length - (j+5)) : 10;
                if (count <= 0) break;
                StripHelper.FillColor(pixels, j + 5, count, Color.Black);
            }
            
            StripHelper.FillColor(pixels, 0, startIndex, Color.Black);

            StripHelper.FillColor(memory, 0, 5, pixelColor);
            StripHelper.FillColor(memory, 5, 10, Color.Black);

            if (startIndex > 10)
                Array.Copy(memory, 0, pixels, 0, startIndex-10);

        }

        public void Tick()
        {
            if (_direction == Direction.Stop)
                return;

            startIndex += 1;
            if (startIndex >= 15)
                startIndex = 0;
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

        private int counter;
        private Color pixelColor;
        private bool rainbowEnabled;
        private DateTime startTime;
        private int startIndex;      
        private int _delay;
        private int _arg;
        private Direction _direction;
        private Color[] memory;            
    }
     
}
