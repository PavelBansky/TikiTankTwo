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
            Forward,
            Backward
        }

        public SimpleTread()
        {         
            this.Argument ="1";
            this.Color = Color.FromArgb(255, 255, 255);
        }

        public void Activate(Color[] pixels)
        {            
            for (int j = 0; j < pixels.Length; j += 15)
            {   
                // Fill five pixel with color
                for (int redi = 0; redi < 5; redi++)
                {
                    pixels[j + redi] =  this.Color;
                }
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
                DisplayHelper.RotateRight(pixels);
            else
                DisplayHelper.RotateLeft(pixels);

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
                    if (i < 0)
                        _direction = Direction.Backward;
                    else if (i > 0)
                        _direction = Direction.Forward;

                    _delay = 400 / Math.Abs(i);
                    _arg = i;
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
