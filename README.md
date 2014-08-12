Tiki Tank Two 
=============

Effect Display Devices
----------------------

Currently we have three output devices for the effects:

- **Threads**: 480 pixels in circle
- **Barrel**: 77 pixels straight line
- **Panels**: 10 LED strips on each side of the tank, represented by the array of 10 pixels

Creating effects
----------------------------

Every effect implements simple *IEffect* interface, with following signature.

    public interface IEffect
    {
        void Activate(System.Drawing.Color[] pixels);
        void Deactivate(System.Drawing.Color[] pixels);
        int Update(System.Drawing.Color[] pixels);
        
		string Argument { get; set; }
        System.Drawing.Color Color { get; set; }
	}

### *void Activate(System.Drawing.Color[] pixels)* ###

Method is called when the effect is activated for showing. Array of colors *pixels[]* contains current content of the display device (last frame). Make changes to this array in order to prepare your first frame, or to make a copy of the current frame to blend it with your effect.

### *void Deactivate(System.Drawing.Color[] pixels)* ###

Method is called when the effect is being deactivated. Array of colors *pixels[]* contains current content of the display device (last frame). Make changes to this array in order to create a last frame. You can use this method to free the additional resources that effect was using.

### *int Update(System.Drawing.Color[] pixels)* ###

This is the main method for the effect which is called by the *frame scheduler*. Array of colors *pixels[]* contains current content of the display device (last frame). Modify this array to create a new frame to be displayed. Method ***returns an integer value*** which represent delay in which you are expecting the *Update* method to be called again by the scheduler. If the Tank is in *"Speed Sensor Mode"* this value is ignored and the *Update()* method is called based on the speed of the vehicle. 

### *string Argument* ###

Property contains user changeable parameter for the effect. 

### *System.Drawing.Color Color* ###

Property contain user changeable color parameter for the effect.

## Sample Effect ##

This is simple effect that blinks the whole strip of LEDs

    public class Blink : IEffect
    {
        public Blink()
        {
            Color = Color.Red;
            darkFrame = false;
        }

        public void Activate(System.Drawing.Color[] pixels)
        {
            // No need for first frame
        }

        public void Deactivate(System.Drawing.Color[] pixels)
        {
            // No need for last frame
        }

        public int Update(System.Drawing.Color[] pixels)
        {
            // Use current user selected color
            Color activeColor = Color;

            // Unless we are in the frame that needs to be black
            if (darkFrame)
                activeColor = Color.Black;

            // Fill the strip with color
            for (int i = 0; i < pixels.Length; i++ )
            {
                pixels[i] = activeColor;
            }

            // Switch for the next frame
            darkFrame = !darkFrame;

            // Request to be called again in 500 milliseconds
            return 500;
        }

        public string Argument { get; set; }
        public System.Drawing.Color Color { get; set; }

        bool darkFrame;
    }	
