using System;
using Nancy;
using Nancy.TinyIoc;
using TikiTankServer.Modules;

namespace TikiTankServer
{
	public class Bootstrapper : DefaultNancyBootstrapper
	{
		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			container.Register<EffectApi>();
			container.Register<SettingsApi>();
			container.Register<Web>();
		}
	}
}
