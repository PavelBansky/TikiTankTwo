using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System;

namespace TikiTank
{
[DataContract]
public class TreadEffect : IEffect
{
	public TreadEffect()
	{
		MetersPerTick = 4.0 / 39.0; // 4"
		TreadColor = "Gray";
	}

    void Activate(System.Drawing.Color[] pixels) 
    {
		metersTraveled = 0.0;
		metersShown = 0.0;
		last = DateTime.Now;
    }

    void Deactivate(System.Drawing.Color[] pixels) {}
    
	bool IsSensorDriven { get; set; }
	string Argument { get; set; }
	System.Drawing.Color Color { get; set; }

	public void Tick()
	{
		// 4 inches per tick, in meters
		metersTraveled += MetersPerTick;
        Console.WriteLine("MetersPerTick {0},  metersTraveled {1}", MetersPerTick, metersTraveled);
	}

	public bool WouldUpdate()
	{
		if( metersShown > metersTraveled )
			return false;
		return true;
	}

	public void FrameUpdate(Color[] pixels)
	{
		Color color = TreadEffect.Color;
		System.Diagnostics.Debug.Assert(0 != color.ToArgb());

		// move the treads about 5% of the way toward the goal each frame
		// while this does lag a bit, it is so simple that it's attractive
		// and it speeds up exponentially rather than continuing to lag
		metersShown = (metersShown * 15 + metersTraveled)/16;

		double pixelSize = 1.0 / 32; // 32 pixels per meter
        // OLD: int offset = (int)(metersShown / pixelSize) % pixels.Length;
        int offset = (int)(metersShown / pixelSize);
        //Console.WriteLine("offset: {0}", offset);
        double overage = (metersShown / pixelSize) - offset;
        //Console.WriteLine("overage: {0}", overage);
        int actualOffset = offset % pixelSize;
        double underage = 1 - overage;

		for( int i = 0; i < pixels.Length; i++ ){
		    // shift by offset amount, modulo wraps the design
            int n = (i + offset) % pixels.Length;

			switch( i % 16 )
			{
				default:
                    //mask by default
					pixels[n] = Color.Black; 
					break;
				// paint 5 pixels solid color
				case 14: continue;
				case 13: continue;
				case 12: continue;
				case 11: continue;
				case 10: continue;
                case 9: 
                    pixels[n] = color; 
					break;
                // paint the edge pixels half-brightness
                // removing for now to replace with percentage for half-positions
				case 15:
                    pixels[n] = Color.FromArgb(
                    color.R * overage, color.G * overage, color.B * overage);
                case 8:
                    pixels[n] = Color.FromArgb(
                    color.R * underage, color.G * underage, color.B * underage);

					break;
			}
		}
	}

	[DataMember]
	public string OutputDevice { get; set; }

	[DataMember]
	public double MetersPerTick { get; set; }

	private DateTime last;
	private double metersTraveled;
	private double metersShown;
}
}