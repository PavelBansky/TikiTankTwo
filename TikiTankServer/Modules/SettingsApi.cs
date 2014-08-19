using Nancy;
using TikiTankServer.Managers;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class SettingsApi : NancyModule
    {
        public SettingsApi(ISettingsService settingsService) : base("/settings")
        {
            Get["/"] = _ =>
            {
                SettingsData data = settingsService.GetSettings();
                return Response.AsJson<SettingsData>(data).WithStatusCode(HttpStatusCode.OK);                
            };

            // Set color, argument, sensorDrievr for given effect
            Post["/"] = _ =>
            {
                if (Request.Form.dmxbrightness.HasValue)
                    settingsService.SetDMXBrightness((string)Request.Form.dmxbrightness);

                if (Request.Form.manualtick.HasValue)
                    settingsService.SetManualTick((string)Request.Form.manualtick);

                if (Request.Form.idleinterval.HasValue)
                    settingsService.SetScreenSaverInterval((string)Request.Form.idleinterval);

                return HttpStatusCode.OK;
            };
        }
    }
}