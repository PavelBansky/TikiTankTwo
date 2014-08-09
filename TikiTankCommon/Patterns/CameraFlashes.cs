using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System;

namespace TikiTankCommon.Patterns
{
public class CameraFlashes : IPattern
{
	public CameraFlashes()
	{
		OutputDevice = "null";
		Delay = 125; // 8 flash per sec

		last = DateTime.Now;
		rng = new Random();
		memory = null;
	}

	public bool WouldUpdate(int frame)
	{
		// stabilize the image by only drawing every other frame
		return frame % 2 == 0;
	}

	public void Update(int frame, Color[] pixels)
	{
		// keep a copy of the correct output state
		if( memory == null || memory.Length != pixels.Length )
			memory = new Color[pixels.Length];

		// first decay previous frame lights
		for( int i = 0; i < pixels.Length; i++ )
		{
			Color l = i == 0 ? pixels[pixels.Length-1] : pixels[i-1];
			Color m = pixels[i];
			Color r = i == pixels.Length-1 ? pixels[0] : pixels[i+1];

			// lossy blur
			memory[i] = Color.FromArgb( 
				(l.R + m.R + r.R) / 4, 
				(l.G + m.G + r.G) / 4, 
				(l.B + m.B + r.B) / 4
				);
		}

		// next add a new camera flash if it's been long enough
		if( DateTime.Now > last + TimeSpan.FromMilliseconds(Delay) )
		{
			int i = rng.Next(0, memory.Length);

			// 3 pixels wide flash, more pixels last longer
			memory[i] = Color.White;
			if( i == 0)
				memory[memory.Length-1] = Color.LightGray;
			else
				memory[i-1] = Color.LightGray;

			if( i == memory.Length-1)
				memory[0] = Color.LightGray;
			else
				memory[i+1] = Color.LightGray;

			last = DateTime.Now;
		}

		Array.Copy( memory, pixels, pixels.Length );
	}

	[DataMember]
	public string OutputDevice { get; set; }

	[DataMember]
	public int Delay { get; set; }

	private Color[] memory;
	private Random rng;
	private DateTime last;
}
}