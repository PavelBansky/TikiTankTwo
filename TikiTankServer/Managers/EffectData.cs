using TikiTankCommon;

namespace TikiTankServer.Managers
{
    public class EffectData : EffectInfo
    {
        public EffectData(EffectInfo info)
        {
            this.Name = info.Name;
            this.ArgumentDescription = info.ArgumentDescription;
            this.Description = info.Description;
            this.IsScreenSaver = info.IsScreenSaver;            
        }
        public int Id { get; set; }
        public string Color;
        public string Argument;
        public bool IsSensorDriven;        
    }
}
