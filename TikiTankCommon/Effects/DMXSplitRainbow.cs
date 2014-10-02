using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class DMXSplitRainbow : IEffect
    {
		class ColorTracker
		{
			int _counter;
			bool _increase;

			public ColorTracker(int start)
			{
				_counter = start;
				_increase = true;
			}

			public Color NextColor()
			{
				var ret = ColorHelper.Wheel(_counter);

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

				return ret;
			}
		}

		ColorTracker[] tracker;

        public DMXSplitRainbow() 
        {
            startTime = DateTime.Now;
        }

		public void Activate(Color[] pixels)
		{
			tracker = new ColorTracker[pixels.Length];
			for (int i = 0; i < tracker.Length; ++i)
				tracker[i] = new ColorTracker(35 * i);
		}

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
                pixels[(pixels.Length - 1) - i] = tracker[i].NextColor();
            }
        }

        public void Tick()
        {
        }

        public bool IsSensorDriven { get; set; }

        public string Argument { get; set;}

        public Color Color {get; set; }

        private DateTime startTime;
    }
}
