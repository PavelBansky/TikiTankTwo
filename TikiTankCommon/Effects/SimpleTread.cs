using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TikiTankCommon.Effects
{
    public class SimpleTread : Effect
    {
        enum Direction
        {            
            Forward,
            Backward
        }

        public SimpleTread(EffectInfo info, LEDStrip strip)
        {
            this.Information = info;
            this.LedStrip = strip;            
            SetArgument("5");
            SetColor(Color.FromRgb(255, 255, 255));
        }

        public override void Activate()
        {
            for (int j = 0; j < LedStrip.Length; j += 15)
            {   
                // Fill five pixel with color
                for (int redi = 0; redi < 5; redi++)
                {
                    LedStrip.SetPixelColor(j + redi, this.GetColor());
                }
                for (int gri = 0; gri < 10; gri++)
                {
                    LedStrip.SetPixelColor(j + gri + 5, 0);
                }
            }
        }

        public override void Deactivate()
        {
            //
        }

        public override int Step()
        {
            if (_direction == Direction.Forward)
                LedStrip.RotateRight();
            else
                LedStrip.RotateLeft();

            LedStrip.Show();

            return _delay;
        }

        public override void SetColor(Color color)
        {
            _color = color;
            Activate();
        }

        public override Color GetColor()
        {
            return _color;
        }

        public override void SetArgument(string argument)
        {
            int i;            
            if (int.TryParse(argument, out i))
            {                
                if (i < 0)
                    _direction = Direction.Backward;
                else if (i > 0)
                    _direction = Direction.Forward;

                _delay = 400 / Math.Abs(i);
                _arg = i;
            }
        }

        public override string GetArgument()
        {
            return _arg.ToString();
        }

        private Color _color;        
        private int _delay;
        private int _arg;
        private Direction _direction;
    }
}
