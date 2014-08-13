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
        void FrameUpdate(System.Drawing.Color[] pixels);
        bool WouldUpdate();
        void Tick();

        bool IsSensorDriven { get; set; }
        string Argument { get; set; }
        System.Drawing.Color Color { get; set; }
	}

### *void Activate(System.Drawing.Color[] pixels)* ###

Method is called when the effect is activated for showing. Array of colors *pixels[]* contains current content of the display device (last frame). Make changes to this array in order to prepare your first frame, or to make a copy of the current frame to blend it with your effect.

### *void Deactivate(System.Drawing.Color[] pixels)* ###

Method is called when the effect is being deactivated. Array of colors *pixels[]* contains current content of the display device (last frame). Make changes to this array in order to create a last frame. You can use this method to free the additional resources that effect was using.

### *int FrameUpdate(System.Drawing.Color[] pixels)* ###

This is the main method for the effect which is called by the *frame scheduler*. Array of colors *pixels[]* contains current content of the display device (last frame). Modify this array to create a new frame to be displayed. 

### *bool WouldUpdate()* ###

Method is called regularly by *frame scheduler* to determine whether the effect needs frame update or not. If the method returns true then ***FrameUpdate()*** is called.  

### *void Tick()* ###

Method is called based on the *Vehicle Speed Sensor* output. When the vehicle is not moving the method is not called. 


### bool *IsSensorDriven* ###

Property is *True* when user requested this effect to be driven by the Vehicle Speed Sensor. When this property is set and vehicle is moving, method *Tick()* is called based on the vehicle speed. 

### *string Argument* ###

Property contains user changeable parameter for the effect. 

### *System.Drawing.Color Color* ###

Property contain user changeable color parameter for the effect.

## Sample Effect ##

This is simple effect that blinks one pixel. Position of the blinking pixel is changing based on vehicle speed.

    class BlinkingPoint : IEffect
    {
        public BlinkingPoint()
        {
            this.Color = System.Drawing.Color.Red;             
        }

        public void Activate(System.Drawing.Color[] pixels)
        {
            startTime = DateTime.Now;     
            length = pixels.Length;
            position = 0;
            pointVisible = true;
        }

        public void Deactivate(System.Drawing.Color[] pixels)
        {

        }

        public void FrameUpdate(System.Drawing.Color[] pixels)
        {
            // Clean the strip
            for (int i = 0; i < length; i++)
                pixels[i] = System.Drawing.Color.Black;

            // Show the point
            if (pointVisible)
                pixels[position] = this.Color;

            pointVisible = !pointVisible;
        }

        public bool WouldUpdate()
        {
            // Determine if it's time to update based (every 100 millisecond)
            TimeSpan delta = DateTime.Now - startTime;
            if (delta.TotalMilliseconds > 100)
            {
                startTime = DateTime.Now;
                return true;
            }

            return false;
        }

        public void Tick()
        {
            // Move the point on tick
            position++;
            if (position >= length)
                position = 0;
        }

        public bool IsSensorDriven { get; set; }

        public string Argument { get; set; }

        public System.Drawing.Color Color { get; set; }

        private int position;
        private int length;
        private bool pointVisible;
        private DateTime startTime;
    }	
