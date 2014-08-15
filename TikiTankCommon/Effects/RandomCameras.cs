using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System;

namespace TikiTank
{
public class RandomCameras : IEffect
{
	public RandomCameras()
	{
		DelayRate = 125; // 8 flash per sec
		FlashRate = 125;
		lastFlash = DateTime.Now;
		lastDecay = DateTime.Now;
		rng = new Random();
		memory = new Color[1];

    	colors = new Color[] {
    		Color.Aqua, Color.Azure, Color.Bisque, Color.BlueViolet, 
    		Color.Chartreuse, Color.Coral, CornflowerBlue, Color.Crimson, 
    		Color.Cyan, Color.DeepPink, Color.DeepSkyBlue, Color.FireBrick, 
    		Color.ForestGreen, Color.Fuchsia, Color.Gold, Color.Gold, 
    		Color.Gold, Color.Gold, Color.Green, Color.GreenYellow, 
    		Color.HotPink, Color.IndianRed, Color.Indigo, Color.Lavender,
    		Color.LemonChiffon, Color.LightBlue, Color.LightGreen, 
    		Color.LightPink, Color.LightYellow, Color.Lime, Color.Magenta, 
    		Color.Maroon, Color.MediumOrchid, Color.MediumSeaGreen, 
    		Color.MediumVioletRed, Color.MintCream, Color.Orange,
    		Color.OrangeRed, Color.PeachPuff, Color.Plum, Color.PowderBlue,
    		Color.Red, Color.Salmon, Color.Silver, Color.Silver, Color.Silver, 
    		Color.Silver, Color.Silver, Color.Silver, Color.Silver, Color.Silver,
    		Color.SpringGreen, Color.Gold, Color.Fuschia, Color.Fuschia, 
    		Color.Fuschia, Color.Teal, Color.Turquoise, Color.Violet, Color.Yellow,
    		Color.YellowGreen, Color.White
    	}
	}


    void Activate(System.Drawing.Color[] pixels){
    	lastFlash = DateTime.Now;
    	lastDecay = DateTime.Now;
    }

    void Deactivate(System.Drawing.Color[] pixels){}

	public bool WouldUpdate()
	{
		// stabilize the image by only drawing every other frame
//		return DateTime.Now > lastDecay + TimeSpan.FromMilliseconds(DecayRate);
		TimeSpan since = DateTime.Now - lastDecay;
		return (since > delay);
	}

	public void FrameUpdate(Color[] pixels)
	{
		// keep a copy of the correct output state
		Color[] nextframe = new Color[pixels.Length];
		lastDecay = DateTime.Now;

		// first decay previous frame lights
		for( int i = 0; i < Math.Min(memory.Length, pixels.Length); i++ )
		{
			Color l = i == 0 ? memory[memory.Length-1] : memory[i-1];
			Color m = pixels[i];
			Color r = i == memory.Length-1 ? memory[0] : memory[i+1];

			// lossy blur
			memory[i] = Color.FromArgb( 
				(l.R + m.R + r.R) / 4, 
				(l.G + m.G + r.G) / 4, 
				(l.B + m.B + r.B) / 4
				);
		}

		// next add a new camera flash if it's been long enough
		if( DateTime.Now > lastFlash + TimeSpan.FromMilliseconds(FlashRate) )
		{
			int i = rng.Next(0, nextframe.Length);

			// 3 pixels wide flash, more pixels last longer
			nextframe[i] = colors[rng.Random(0,colors.Length)];

			lastFlash = DateTime.Now;
		}

		Array.Copy( memory, pixels, pixels.Length );
	}

    void Tick(){}
	bool IsSensorDriven { get; set; }
	string Argument { get; set; }
	System.Drawing.Color Color { get; set; }

	[DataMember]
	public string OutputDevice { get; set; }

	[DataMember]
	public int FlashRate { get; set; }
	public int DecayRate { get; set; }
	private Color[] memory;
	private Random rng;
	private DateTime last;

	private Color[] colors;
}
}