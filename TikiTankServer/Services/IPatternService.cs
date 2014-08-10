using System.Collections.Generic;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public interface IPatternService
    {
        List<EffectInformation> GetEffectsInformation();
        void SetEffect(string index);
        //void SetColor(string color);
        //void SetArgument(string argument);
        //void SetSensorDrive(string sensorDrive);
    }
}
