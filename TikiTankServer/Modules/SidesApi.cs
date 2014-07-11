using Nancy;
using System;
using System.Text;
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
                //Effect eff = new Effect() { Name = "Duha", Color = "FF00FF", Arg = "123" };

                return HttpStatusCode.OK;
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
