using System;
using System.Drawing;

namespace TikiTankCommon.Effects
{
public class Glow : IEffect
{
	public Glow()
	{		
		DelayRate = 125; // 8 flash per sec
		GlowRate = 125;
		lastGlow = DateTime.Now;
		lastDecay = DateTime.Now;
		rng = new Random();
		memory = new Color[1];
		offsetCount = 25;
		offset = new int[offsetCount];
		step = 5;

	}


    public void Activate(System.Drawing.Color[] pixels){
    	lastGlow = DateTime.Now;
    	lastDecay = DateTime.Now;

    	for (int i = 0; i < offsetCount; i++){
    		offset[i] = rng.Next(0,pixels.Length);
    	}
    }

    public void Deactivate(System.Drawing.Color[] pixels){}

	public bool WouldUpdate()
	{
		TimeSpan since = DateTime.Now - lastDecay;
		return (since > TimeSpan.FromMilliseconds(DelayRate));
	}

	public void FrameUpdate(Color[] pixels)
	{
		// replace a random offset with a random pixel position
		int dropped = rng.Next(0, offsetCount);
		offset[dropped] = rng.Next(0, pixels.Length);

		// keep a copy of the correct output state
		Color[] nextframe = new Color[pixels.Length];
		lastDecay = DateTime.Now;

		// first decay previous frame lights
		for( int i = 0; i < Math.Min(memory.Length, pixels.Length); i++ )
		{
			Color l = i == 0 ? memory[memory.Length-1] : memory[i-1];
			Color m = pixels[i];
			Color r = i == memory.Length - 1 ? memory[0] : memory[i+1];

			// lossy blur
			nextframe[i] = Color.FromArgb( 
				(l.R + m.R + r.R + this.Color.R) / 4, 
				(l.G + m.G + r.G + this.Color.G) / 4, 
				(l.B + m.B + r.B + this.Color.B) / 4
				);
		}

		// next add a new camera flash if it's been long enough
		if( DateTime.Now > lastGlow + TimeSpan.FromMilliseconds(GlowRate) )
		{
			for (int i = 0; i < offset.Length; i++){
				int position = offset[i];
                pixels[position] = Color.FromArgb(Math.Min(255, pixels[position].R + step), Math.Min(255, pixels[position].G + step), Math.Min(255, pixels[position].B + step));
			}
			lastGlow = DateTime.Now;
		}

		Array.Copy( nextframe, pixels, pixels.Length );
        memory = nextframe;
	}

    public void Tick(){}
	public bool IsSensorDriven { get; set; }
    public string Argument { get { return offsetCount.ToString(); } set { offsetCount = Convert.ToInt32(value); offset = new int[offsetCount]; } }

	public System.Drawing.Color Color { get; set; }


	public int GlowRate { get; set; }
	public int DecayRate { get; set; }
	private Color[] memory;
	private Random rng;

	int	DelayRate = 125; // 8 flash per sec		
    DateTime lastGlow = DateTime.Now;
    DateTime lastDecay = DateTime.Now;
		

	int offsetCount = 5;
	int[] offset = new int[5];
	int	step = 5;
}
}