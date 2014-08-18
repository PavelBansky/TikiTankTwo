using System.Drawing;
using System;

namespace TikiTankCommon.Effects
{
    public class Gradient : IEffect
    {
	    public Gradient()
	    {
	    }

        public void Activate(System.Drawing.Color[] pixels)
        {
    	    focusPixel = 0;
    	    delay = TimeSpan.FromSeconds( 3.0 / pixels.Length ); // 3 seconds per cycle
        }
    
        public  void Deactivate(System.Drawing.Color[] pixels)
        {}
	
	    public bool WouldUpdate()
	    {
		    return last + delay > DateTime.Now;
	    }

	    public void Tick(){}

	    public void FrameUpdate(Color[] pixels)
	    {
		    // compute the focal pixel
            focusPixel += (int)((DateTime.Now - last).TotalMilliseconds / delay.TotalMilliseconds);
		    last = DateTime.Now;

		    for( int i = 0; i < pixels.Length; i++ )
		    {
			    // rotate the index around
			    int n = (focusPixel + i) % pixels.Length;

			    // fade function
			    double brightness = Math.Cos( 2 * Math.PI * (double)n / (double)pixels.Length );

			    // brightness += up to half way to white
			    pixels[i] = Color.FromArgb((int)(this.Color.R + (255 - this.Color.R) * (brightness/2)), 
                                           (int)(this.Color.G + (255 - this.Color.G) * (brightness/2)), 
                                           (int)(this.Color.B + (255 - this.Color.B) * (brightness/2)));
		    }
	    }

        public bool IsSensorDriven { get; set; }
        public string Argument { get; set; }
        public Color Color { get; set; }

	    private DateTime last;
	    private TimeSpan delay;
	    private int focusPixel;
    }
}