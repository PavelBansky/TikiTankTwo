namespace TikiTankCommon
{
    public interface IPattern
    {

        /// <summary>
        /// Name of the pattern the way it will show in Web UI
        /// </summary>
        string Name { get; }

        /// <summary>
        /// True if the pattern is driven by the speed sensor
        /// </summary>
        bool IsSensorDriven { get; }
	    string OutputDevice { get; }
	    bool WouldUpdate(int frame);
	    void Update(int frame, System.Drawing.Color[] pixels);

    }
}