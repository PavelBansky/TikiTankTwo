using System;
using System.Drawing;

namespace TikiTankHardware
{
    public interface IDisplayDevice : IDisposable
    {
        void Show(Color[] pixels);

        int Length { get; }
    }
}
