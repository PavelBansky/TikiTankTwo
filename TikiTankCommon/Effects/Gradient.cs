using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System;

namespace TikiTank
{
public class Glow : IEffect
{
	public Glow()
	{
	}

    void Activate(System.Drawing.Color[] pixels)
    {
    	focusPixel = 0;
    	delay = TimeSpan.FromSeconds( 3.0 / pixels.Length ); // 3 seconds per cycle
    }
    
    void Deactivate(System.Drawing.Color[] pixels)
    {}
	
	public bool WouldUpdate()
	{
		return last + delay > DateTime.Now;
	}

	void Tick(){}

	public void FrameUpdate(Color[] pixels)
	{
		// compute the focal pixel
		focusPixel += (DateTime.Now - last).TotalMicroseconds() / delay.TotalMicroseconds();
		last = DateTime.Now;

		for( int i = 0; i < pixels.Length; i++ )
		{
			// rotate the index around
			int n = (focusPixel + i) % pixels.Length;

			// fade function
			double brightness = Math.Cos( 2 * Math.PI * (double)n / (double)pixels.Length );

			// brightness += up to half way to white
			pixels[i].R = Glow.Color.R + (255 - Glow.Color.R) * (brightness/2);
			pixels[i].G = Glow.Color.G + (255 - Glow.Color.G) * (brightness/2);
			pixels[i].B = Glow.Color.B + (255 - Glow.Color.B) * (brightness/2);
		}
	}

	private DateTime last;
	private TimeSpan delay;
	private int focusPixel;
}
}