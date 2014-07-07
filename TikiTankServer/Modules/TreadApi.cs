using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class Effect
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Arg { get; set; }
    }

    public class TreadApi : NancyModule
    {
        public TreadApi(ITreadService treadService) : base("/api/tread")
        {
            Get["/effect"] = _ =>
                                    {
                                        Effect eff = new Effect() { Name = "Duha", Color = "FF00FF", Arg = "123" };

                                        return Response.AsJson<Effect>(eff);
                                    };

            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
                                    {
                                        treadService.SetEffect(parameters.effect);
                                        return HttpStatusCode.OK;
                                    };

            // Set color for given effect
            Post["/effect"] = _ =>
                                    {
                                        if (Request.Form.color.HasValue)
                                            treadService.SetColor((string)Request.Form.color);

                                        if (Request.Form.argument.HasValue)
                                            treadService.SetArgument((string)Request.Form.argument);

                                        return HttpStatusCode.OK;
                                    };
        }
    }
}
