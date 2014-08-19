using System;
using Nancy;
using Nancy.TinyIoc;
using TikiTankServer.Modules;
using TikiTankServer.Services;

namespace TikiTankServer
{
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
}
