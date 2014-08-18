using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TikiTankCommon
{
	public class BaseTreadEffect
	{
		public BaseTreadEffect()
		{
			direction = Direction.Stop;
				delay = 0;
		}
		public enum Direction
		{
			Stop = 0,
			Forward = 1,
			Backward = -1
		}

		public string Argument
		{
			get
			{
				return ((int)direction).ToString();
			}
			set
			{
				try
				{
					direction = (Direction)int.Parse(value);

					if (Math.Abs((int)direction) > 0)
						delay = 400 / Math.Abs((int)direction);
				}
				catch
				{
					Console.WriteLine("Invalid arg to tread effect. Want -1, 0, 1.");
				}
			}
		}

		public virtual void Activate(Color[] pixels) { }

		public virtual void Deactivate(Color[] pixels) { }

		public virtual bool WouldUpdate()
		{
			return true;
		}

		public virtual void FrameUpdate(Color[] pixels) { }

		protected int delay {get;set;}
		protected Direction direction {get;set;}
		public bool IsSensorDriven { get; set; }
		public Color Color { get; set; }
	}
}
