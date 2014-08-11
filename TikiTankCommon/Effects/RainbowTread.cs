using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class RainbowTread : IEffect
    {
        enum Direction
        {            
            Stop,
            Forward,
            Backward            
        }

        public RainbowTread()
        {         
            this.Argument ="0";
            this.Color = Color.FromArgb(255, 255, 255);
        }

        public void Activate(Color[] pixels)
        {            
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[(pixels.Length - 1) - i] = ColorHelper.Wheel(((i * 384 / pixels.Length) ) % 384);
            }

            for (int j = 0; j < pixels.Length; j += 15)
            {   
                for (int gri = 0; gri < 10; gri++)
                {
                    pixels[j + gri + 5] = Color.Black;
                }
            }
        }

        public void Deactivate(Color[] pixels) { }

        public int Update(Color[] pixels)
        {
            if (_activationNeeded)
            {
                Activate(pixels);
                _activationNeeded = false;
            }

            if (_direction == Direction.Forward)
                StripHelper.RotateRight(pixels);
            else if (_direction == Direction.Backward)
                StripHelper.RotateLeft(pixels);

            return _delay;
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                _activationNeeded = true;
            }
        }

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

        private bool _activationNeeded;
        private Color _color;        
        private int _delay;
        private int _arg;
        private Direction _direction;
    }
}
