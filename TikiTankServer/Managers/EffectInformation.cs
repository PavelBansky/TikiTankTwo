using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankCommon;

namespace TikiTankServer.Managers
{
    public class EffectInformation : EffectInfo
    {
        public EffectInformation(EffectInfo info)
        {
            this.Name = info.Name;
            this.ArgumentDescription = info.ArgumentDescription;
            this.Description = info.Description;
        }
        public int Id { get; set; }
    }
}
