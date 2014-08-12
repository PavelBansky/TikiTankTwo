using System.Collections.Generic;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public interface IEffectService
    {
        List<EffectData> GetEffectsInformation();
        EffectData SetEffect(string index);
        void SetColor(string color);
        void SetArgument(string argument);
        void SetSensorDrive(string sensorDrive);
    }
}
