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
                return HttpStatusCode.OK;
            };

            Get["/effects"] = _ =>
            {
                return Response.AsJson(panelsService.GetEffectsInformation().ToArray());
            };

            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
            {
                panelsService.SetEffect(parameters.effect);
                return HttpStatusCode.OK;
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
