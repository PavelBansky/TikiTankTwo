using System.Collections.Generic;
using TikiTankServer.Managers;

namespace TikiTankServer.Services
{
    public interface IEffectService
    {
        List<EffectData> GetEffectsInformation();
        EffectData SetEffect(string index);

        EffectData GetEffect();
        
        void SetColor(string color);
        void SetArgument(string argument);        
    }
}
