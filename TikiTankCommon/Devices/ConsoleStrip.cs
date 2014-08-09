using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System;

namespace TikiTankCommon.Devices
{

[DataContract]
public class ConsoleStrip : IShowable
{
	public ConsoleStrip()
	{
	}

    public bool Init()
    {
        return true;
    }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Filename { get; set; }

	[DataMember]
	public int Pixels { get; set; }

	public void Show(Color[] pixels)
	{
		Debug.Assert(pixels.Length == Pixels);

		foreach( Color c in pixels )
		{
			Console.ForegroundColor = ClosestConsoleColor(c.R, c.G, c.B);
			Console.Write("#");
		}
		Console.WriteLine();
		Console.ResetColor();
	}

	static ConsoleColor ClosestConsoleColor(byte r, byte g, byte b)
    {
        ConsoleColor ret = 0;
        double rr = r, gg = g, bb = b, delta = double.MaxValue;

        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
        {
            var n = Enum.GetName(typeof(ConsoleColor), cc);
            var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
            var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
            if (t == 0.0)
                return cc;
            if (t < delta)
            {
                delta = t;
                ret = cc;
            }
        }
        return ret;
    }
}

}