using Nancy;
using System;
using System.Collections.Generic;
using System.Text;
using TikiTankServer.Managers;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class PanelsApi : NancyModule
    {
        public PanelsApi(IPanelsService panelsService)
            : base("/api/panels")
        {
            Get["/effect"] = _ =>
            {
                EffectData data = panelsService.GetEffect();
                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

            Get["/effects"] = _ =>
            {
                return Response.AsJson(panelsService.GetEffectsInformation().ToArray());
            };

            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
            {
                EffectData data = panelsService.SetEffect(parameters.effect);
                Response.AsJson<EffectData>(data);

                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

            // Set color for given effect
            Post["/effect"] = _ =>
            {
                if (Request.Form.argument.HasValue)
                    panelsService.SetArgument((string)Request.Form.argument);

                if (Request.Form.color.HasValue)
                    panelsService.SetColor((string)Request.Form.color);

                return HttpStatusCode.OK;
            };
        }
    }
}
