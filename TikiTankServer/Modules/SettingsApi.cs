using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiTankServer.Modules
{
    public class SettingsApi : NancyModule
    {
        public SettingsApi() : base("/settings")
        {
            Get["/"] = parameters =>
            {
                EffectData data = effectService.GetEffect(parameters.api);
                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

            Get["/{api}/effects"] = parameters =>
            {
                EffectData[] data = effectService.GetEffectsInformation(parameters.api).ToArray();
                return Response.AsJson(data);
            };

            // Set effect for treads
            Post["/{api}/effect/{effect}"] = parameters =>
            {
                EffectData data = effectService.SetEffect(parameters.api, parameters.effect);
                Response.AsJson<EffectData>(data);

                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };

            // Set color, argument, sensorDrievr for given effect
            Post["/{api}/effect"] = parameters =>
            {
                if (Request.Form.argument.HasValue)
                    effectService.SetArgument(parameters.api, (string)Request.Form.argument);

                if (Request.Form.color.HasValue)
                    effectService.SetColor(parameters.api, (string)Request.Form.color);

                if (Request.Form.sensordriven.HasValue)
                    effectService.SetSensorDrive(parameters.api, (string)Request.Form.sensordriven);

                return HttpStatusCode.OK;
            };        
        }
    }
}