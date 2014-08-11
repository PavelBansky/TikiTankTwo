using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TikiTankCommon
{
    public interface IDisplayDevice : IDisposable
    {
        void Show(Color[] pixels);

        int Length { get; }
    }
}
