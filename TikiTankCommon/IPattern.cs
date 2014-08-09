namespace TikiTankCommon
{
    public interface IPattern
    {
	    string OutputDevice { get; }
	    bool WouldUpdate(int frame);
	    void Update(int frame, System.Drawing.Color[] pixels);
    }
}