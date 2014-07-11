using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankServer.Services
{
    public interface ISidesService
    {
        void SetEffect(string index);
        void SetColor(string color);
        void SetArgument(string argument);
    }
}
