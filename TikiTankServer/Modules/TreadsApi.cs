using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikiTankServer.Managers;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class TreadsApi : NancyModule
    {
        public TreadsApi(ITreadsService treadsService) : base("/api/treads")
        {
            Get["/effect"] = _ =>
            {
                EffectData data = treadsService.GetEffect();
                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

            Get["/effects"] = _ =>
            {
                return Response.AsJson(treadsService.GetEffectsInformation().ToArray());
            };

            // Set effect for treads
            Post["/effect/{effect}"] = parameters =>
            {
                EffectData data = treadsService.SetEffect(parameters.effect);
                Response.AsJson<EffectData>(data);

                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

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
        }
    }
}
