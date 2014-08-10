using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class TreadsApi : NancyModule
    {
        public TreadsApi(ITreadsService treadsService) : base("/api/treads")
        {
            Get["/effect"] = _ =>
                                    {
                                       // Effect eff = new Effect() { Name = "Duha", Color = "FF00FF", Arg = "123" };
                                        return HttpStatusCode.OK; // Response.AsJson<Effect>(eff);
                                    };

            Get["/effects"] = _ =>
            {
                return Response.AsJson(treadsService.GetEffectsInformation().ToArray());
            };
            
            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
                                    {
                                        treadsService.SetEffect(parameters.effect);
                                        return HttpStatusCode.OK;
                                    };
            /*
            // Set color, argument, sensorDrievr for given effect
            Post["/effect"] = _ =>
                                    {
                                        if (Request.Form.argument.HasValue)
                                            treadsService.SetArgument((string)Request.Form.argument);

                                        if (Request.Form.color.HasValue)
                                            treadsService.SetColor((string)Request.Form.color);

                                        if (Request.Form.sensordriven.HasValue)
                                            treadsService.SetSensorDrive((string)Request.Form.sensordriven);

                                        return HttpStatusCode.OK;
                                    };
        
             */
        }
    }
}
