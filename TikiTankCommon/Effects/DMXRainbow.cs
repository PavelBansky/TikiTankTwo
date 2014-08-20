using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class DMXRainbow : IEffect
    {
        public DMXRainbow() 
        {
            _counter = 0;
            startTime = DateTime.Now;
            _increase = true;
        }

        public void Activate(Color[] pixels) { }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
            TimeSpan delta = DateTime.Now - startTime;
            if (delta.TotalMilliseconds > 30)
            {
                startTime = DateTime.Now;
                return true;
            }

            return false;
        }

        public void FrameUpdate(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[(pixels.Length - 1) - i] = ColorHelper.Wheel(_counter);
            }

            //cycles of all 384 colors in the wheel
            //_counter = (_counter < 384) ? _counter + 1 : 0;

            //cycles of all 384 colors in the wheel            
            _counter = (_increase) ? _counter + 5 : _counter - 5;


            if (_counter > 382)
            {
                _increase = false;
                _counter = 382;
            }
            else if (_counter < 1)
            {
                _increase = true;
                _counter = 1;
            }
        }

        public void Tick()
        {

        }

        public bool IsSensorDriven { get; set; }

        public string Argument { get; set;}

        public Color Color {get; set; }

        private int _counter;
        private DateTime startTime;
        private bool _increase;
    }
}
