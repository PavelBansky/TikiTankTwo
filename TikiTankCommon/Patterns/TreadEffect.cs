using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System;

namespace TikiTankCommon.Patterns
{
[DataContract]
public class TreadEffect : IPattern
{
	public TreadEffect()
	{
		Period = 100;
		MetersPerTick = 4.0 / 39.0; // 4"
		TreadColor = "Gray";

		metersTraveled = 0.0;
		metersShown = 0.0;
		last = DateTime.Now;
	}

	public void OnTick()
	{
		// 4 inches per tick, in meters
		metersTraveled += MetersPerTick;
	}

	public bool WouldUpdate(int frame)
	{
		// implement static speed control, turn to 0 to rely on interrupts
		if( Period > 0 ) {
			if( DateTime.Now + TimeSpan.FromMilliseconds( Period ) < last ) {
				last = DateTime.Now;
				OnTick();
			}
		}

		if( metersShown > metersTraveled )
			return false;
		return true;
	}

	public void Update(int frame, Color[] pixels)
	{
		Color color = Color.FromName(TreadColor);
		System.Diagnostics.Debug.Assert(0 != color.ToArgb());

		// move the treads about 5% of the way toward the goal each frame
		// while this does lag a bit, it is so simple that it's attractive
		// and it speeds up exponentially rather than continuing to lag
		metersShown = (metersShown * 15 + metersTraveled)/16;

		double pixelSize = 1.0 / 32; // 32 pixels per meter
		int offset = (int)(metersShown / pixelSize) % pixels.Length;

		for( int i = 0; i < pixels.Length; i++ ){
			int n = (i + offset) % pixels.Length;

			switch( i % 16 )
			{
				default: 
					pixels[n] = Color.Black; 
					break;
				
				case 14: continue;
				case 13: continue;
				case 12: continue;
				case 11: continue;
				case 10: continue;
				case 9: 
					pixels[n] = color; 
					break;

				case 15: continue;
				case 8: 
					pixels[n] = Color.FromArgb( 
					color.R / 2, color.G / 2, color.B / 2 );
					break;
			}
		}
	}

	[DataMember]
	public string OutputDevice { get; set; }

	[DataMember]
	public string TreadColor { get; set; }

	[DataMember]
	public int Period { get; set; }

	[DataMember]
	public double MetersPerTick { get; set; }

	private DateTime last;
	private double metersTraveled;
	private double metersShown;
}
}