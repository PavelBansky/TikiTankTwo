using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
	public class AntsTread : BaseTreadEffect, ITreadEffect
    {
		Color[] Pixels;

		public AntsTread() : base()
        {
        }

        public override void Activate(Color[] pixels)
        {
			Color = Color.White;
			Pixels = pixels;

			for (int i = 0; i < pixels.Length; i++)
			{
				if (i % 12 == 0)
				{
					pixels[i+3] = Color.White;
					pixels[i+2] = Color.White.MakeDarker(0.25f);
					pixels[i+1] = Color.White.MakeDarker(0.50f);
					pixels[i+0] = Color.White.MakeDarker(0.75f);
				}
			}
        }

        public void Tick()
        {
			if (direction == Direction.Forward)
				StripHelper.RotateRight(Pixels);
			else if (direction == Direction.Backward)
				StripHelper.RotateLeft(Pixels);
        }
    }
}

// end
