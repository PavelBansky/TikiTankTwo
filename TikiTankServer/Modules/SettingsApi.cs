using Nancy;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class SettingsApi : NancyModule
    {
        public SettingsApi(ISettingsService settingsService) : base("/settings")
        {
            Get["/"] = _ =>
            {
                //EffectData data = sett.GetEffect(parameters.api);
                //return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
                return HttpStatusCode.OK;
            };

            // Set color, argument, sensorDrievr for given effect
            Post["/"] = _ =>
            {
                if (Request.Form.dmxbrightness.HasValue)
                    settingsService.SetDMXBrightness((string)Request.Form.dmxbrightness);


                return HttpStatusCode.OK;
            };
        }
    }
}