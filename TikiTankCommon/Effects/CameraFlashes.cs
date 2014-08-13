using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TikiTankCommon.Effects
{
    public class CameraFlashes : IEffect
    {
        public CameraFlashes()
        {
            this.Argument = "100"; // 100 flashes per second
            last = DateTime.Now;
            rng = new Random();
        }

        public void Activate(Color[] pixels)
        {
            
        }

        public void Deactivate(Color[] pixels)
        {
           
        }

        public int Update(Color[] pixels)
        {

            // keep a copy of the correct output state
            if (memory == null || memory.Length != pixels.Length)
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

            return 0;
        }

        /// <summary>
        /// Number of flashes per second
        /// </summary>
        public string Argument
        {
            get { return _arg.ToString();  }
            set 
            { 
                int i;
                if (int.TryParse(value, out i))
                {
                    if (i > 0)
                    {
                        _arg = i;
                        Delay = 1000 / _arg;
                    }
                }
            }
        }

        public Color Color
        {
            get;
            set;
        }
        private int _arg;
        private int Delay;
	    private Color[] memory;
	    private Random rng;
	    private DateTime last;
    }
}
