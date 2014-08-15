using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon.Effects
{
    public class DMXRainbow : IEffect
    {
        public DMXRainbow() 
        {
            _counter = 0;
            startTime = DateTime.Now;
        }

        public void Activate(Color[] pixels) { }

        public void Deactivate(Color[] pixels) { }

        public bool WouldUpdate()
        {
            TimeSpan delta = DateTime.Now - startTime;
            if (delta.TotalMilliseconds > 50)
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
                pixels[(pixels.Length-1) - i] = ColorHelper.Wheel(((i * 384) + _counter) % 384);
            }

            //cycles of all 384 colors in the wheel
            _counter = (_counter < 384) ? _counter + 1 : 0;
        }

        public void Tick()
        {

        }

        public bool IsSensorDriven { get; set; }

        public string Argument { get; set;}

        public Color Color {get; set; }

        private int _counter;
        private DateTime startTime;
    }
}
