using System.Collections.Generic;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public interface IEffectService
    {
        List<EffectData> GetEffectsInformation(string device);
        EffectData SetEffect(string device, string index);
        EffectData GetEffect(string device);        
        void SetColor(string device, string color);
        void SetArgument(string device, string argument);
        void SetSensorDrive(string device, string sensorDrive);
        void SetAsScreenSaver(string device, string sensorDrive);
    }
}
