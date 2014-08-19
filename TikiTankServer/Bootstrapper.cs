using System;
using Nancy;
using Nancy.TinyIoc;
using TikiTankServer.Modules;
using TikiTankServer.Services;

namespace TikiTankServer
{
<<<<<<< HEAD
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IEffectService, EffectService>();
            container.Register<ISettingsService, SettingsService>();
            container.Register<EffectApi>();
            container.Register<SettingsApi>();
            container.Register<Web>();
        }
    }
=======
	public class Bootstrapper : DefaultNancyBootstrapper
	{
		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			container.Register<IEffectService, EffectService>();
			container.Register<ISettingsService, SettingsService>();
			container.Register<EffectApi>();
			container.Register<SettingsApi>();
			container.Register<Web>();
		}
	}
>>>>>>> d1817113c8095f950a692f0856d76856807628a7
}
