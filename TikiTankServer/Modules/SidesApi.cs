using Nancy;
using System;
using System.Collections.Generic;
using System.Text;
using TikiTankServer.Managers;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class SidesApi : NancyModule
    {
        public SidesApi(ISidesService sidesService)
            : base("/api/sides")
        {
            Get["/effect"] = _ =>
            {
                return HttpStatusCode.OK;
            };

            Get["/effects"] = _ =>
            {
                return Response.AsJson(sidesService.GetEffectsInformation().ToArray());
            };

            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
            {
                sidesService.SetEffect(parameters.effect);
                return HttpStatusCode.OK;
            };

            // Set color for given effect
            Post["/effect"] = _ =>
            {
                if (Request.Form.argument.HasValue)
                    sidesService.SetArgument((string)Request.Form.argument);

                if (Request.Form.color.HasValue)
                    sidesService.SetColor((string)Request.Form.color);

                return HttpStatusCode.OK;
            };
        }
    }
}
