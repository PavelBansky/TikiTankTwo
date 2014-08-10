using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankCommon;

namespace TikiTankServer.Managers
{
    public class EffectInformation
    {
        public EffectInformation(string name)
        {
            this.Name = name;
            this.Description = string.Empty;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
