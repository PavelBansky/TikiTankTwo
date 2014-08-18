using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
	public class FullRainbowTread : BaseTreadEffect, ITreadEffect
    {
		private Color[] memory;
		private DateTime startTime;

		public FullRainbowTread()
			: base()
        {
            this.Color = Color.FromArgb(255, 255, 255);
            startTime = DateTime.Now;
        }

        public override void Activate(Color[] pixels)
        {
			for (int i = 0; i < pixels.Length; i++)
			{
				if (i >= 384)
					pixels[(pixels.Length - 1) - i] = ColorHelper.Wheel(383);
				else
					pixels[(pixels.Length - 1) - i] = ColorHelper.Wheel(i);
			}

			memory = pixels;
        }

        public void Tick()
        {
            if (direction == Direction.Forward)
                StripHelper.RotateRight(memory);
            else if (direction == Direction.Backward)
                StripHelper.RotateLeft(memory);
        }
    }
}
