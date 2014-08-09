using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class BarrelApi : NancyModule
    {
        public BarrelApi(IBarrelService barrelService) : base("/api/barrel")
        {
            Get["/effect"] = _ =>
                                    {
                                       // Effect eff = new Effect() { Name = "Duha", Color = "FF00FF", Arg = "123" };
                                        return HttpStatusCode.OK; // Response.AsJson<Effect>(eff);
                                    };

            Get["/effects"] = _ =>
            {
                return Response.AsJson(barrelService.GetEffectsInformation().ToArray());
            };

            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
                                    {
                                        barrelService.SetEffect(parameters.effect);
                                        return HttpStatusCode.OK;
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
