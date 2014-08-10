using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System;

namespace TikiTankCommon.Patterns
{
[DataContract]
public class Glow : IPattern
{
	public Glow()
	{
		PeakColor="DarkGreen";

		rng = new Random();
	}

	public void Update( int frame, Color[] pixels )
	{
        Color color = ColorHelper.StringToColor(PeakColor); // Color.FromName(PeakColor);
		System.Diagnostics.Debug.Assert(color.ToArgb() != 0);

		Color current = Color.FromArgb(
			color.R - rng.Next(0, color.R/16+1),
			color.G - rng.Next(0, color.G/16+1),
			color.B - rng.Next(0, color.B/16+1)
			);
		for( int i = 0; i < pixels.Length; i++ )
			pixels[i] = current;
	}

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public bool IsSensorDriven { get; set; }

    [DataMember]
    public string OutputDevice { get; set; }

    [DataMember]
    public string PeakColor { get; set; }

    public bool WouldUpdate(int frame)
    {
        return true;
    }

	private Random rng;
}
}