using TikiTankServer.Managers;

namespace TikiTankServer
{
    public static class TankManager
    {
        static TankManager()
        {
            TreadsManager = new EffectManager();
            BarrelManager = new EffectManager();
            SidesManager = new EffectManager();            
        }

        public static EffectManager TreadsManager { get; set; }
        public static EffectManager BarrelManager { get; set; }
        public static EffectManager SidesManager { get; set; }
    }
}
