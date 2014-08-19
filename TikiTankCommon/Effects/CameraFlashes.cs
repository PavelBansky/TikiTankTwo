using System.Runtime.Serialization;
using System.Drawing;
using System;

namespace TikiTankCommon.Effects
{
public class CameraFlashes : IEffect
{
	public CameraFlashes()
	{
		DecayRate = 50;
        Argument = "8"; // 8 flash per sec
		lastFlash = DateTime.Now;
		lastDecay = DateTime.Now;
		rng = new Random();
		memory = new Color[1];
        this.Color = Color.White;
	}


    public void Activate(System.Drawing.Color[] pixels)
    {
        lastFlash = DateTime.Now;
        lastDecay = DateTime.Now;
        memory = new Color[pixels.Length];
    }

    public void Deactivate(System.Drawing.Color[] pixels){}

	public bool WouldUpdate()
	{
        return DateTime.Now > lastFlash || DateTime.Now > lastDecay;
	}

	public void FrameUpdate(Color[] pixels)
	{
		// next add a new camera flash if it's been long enough
        while (DateTime.Now > lastDecay)
        {
            // first decay previous frame lights
            for (int i = 0; i < Math.Min(memory.Length, pixels.Length); i++)
            {
                Color l = i == 0 ? memory[memory.Length - 1] : memory[i - 1];
                Color m = memory[i];
                Color r = i == memory.Length - 1 ? memory[0] : memory[i + 1];

                // lossy blur
                memory[i] = Color.FromArgb(
                    (l.R + m.R + r.R) / 4,
                    (l.G + m.G + r.G) / 4,
                    (l.B + m.B + r.B) / 4
                    );
            }

            lastDecay += TimeSpan.FromMilliseconds(DecayRate);
        }

        while (DateTime.Now > lastFlash)
        {
            // next add a new camera flash if it's been long enough
            int i = rng.Next(0, memory.Length);

            // 3 pixels wide flash, more pixels last longer
            memory[i] = Color; // Color.White;

            lastFlash += TimeSpan.FromMilliseconds(_flashRate);
        }

        Array.Copy(memory, pixels, pixels.Length);
	}

    public void Tick(){}
	public bool IsSensorDriven { get; set; }
	public string Argument 
    {
        get
        {
            return _arg.ToString();
        }
        set 
        { 
            int i;
            if (int.TryParse(value, out i))
            {
                _arg = i;
                _flashRate = 1000 / _arg;
            }
        } 
    }
	public System.Drawing.Color Color { get; set; }

	public int DecayRate { get; set; }
	
    private Color[] memory;
	private Random rng;
    private DateTime lastDecay, lastFlash;
    private int _flashRate, _arg;
}
}