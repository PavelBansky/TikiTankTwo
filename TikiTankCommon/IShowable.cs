using System.Drawing;

namespace TikiTankCommon
{
    public interface IShowable
    {
	    string Name { get; }
	    int Pixels { get; }

        bool Init();
        void Show(Color[] pixels);
    }
}