using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankServer.Managers;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class BarrelApi : NancyModule
    {
        public BarrelApi(IBarrelService barrelService) : base("/api/barrel")
        {
            Get["/effect"] = _ =>
            {
                EffectData data = barrelService.GetEffect();
                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

            Get["/effects"] = _ =>
            {
                return Response.AsJson(barrelService.GetEffectsInformation().ToArray());
            };

            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
            {
                EffectData data = barrelService.SetEffect(parameters.effect);
                Response.AsJson<EffectData>(data);

                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

            // Set color for given effect
            Post["/effect"] = _ =>
            {
                if (Request.Form.argument.HasValue)
                    barrelService.SetArgument((string)Request.Form.argument);

                if (Request.Form.color.HasValue)
                    barrelService.SetColor((string)Request.Form.color);

                return HttpStatusCode.OK;
            };
        }
    }    
}
