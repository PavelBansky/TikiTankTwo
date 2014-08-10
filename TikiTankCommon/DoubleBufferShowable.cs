using System;
using System.Drawing;

namespace TikiTankCommon
{
    class DoubleBufferShowable
    {
	    public DoubleBufferShowable( 
		    string _name, IShowable _dev, int _len )
	    {
		    Name = _name;
		    Device = _dev;
		    front = new Color[_len];
		    back = new Color[_len];
		    frontNext = true;
	    }        

	    public string Name {get; private set;}
	    public IShowable Device {get; private set;}

	    public Color[] DrawFrame 
	    { get {
		    if( frontNext )
			    return front;
		    else
			    return back;
	    }}

	    public void Show()
	    {
		    frontNext = !frontNext;
		    if( frontNext )
			    Device.Show( back );
		    else
			    Device.Show( front );
	    }

	    private Color[] front;
	    private Color[] back;
	    private bool frontNext;
    }
}