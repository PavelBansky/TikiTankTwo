using Nancy;
using TikiTankServer.Managers;
using TikiTankServer.Services;

namespace TikiTankServer.Modules
{
    public class EffectApi : NancyModule
    {
        public EffectApi(IEffectService effectService) : base("/api")
        {
            Get["/"] = _ =>
            {
                EffectData data = effectService.GetEffect(parameters.api);
                return Response.AsJson<EffectData>(data).WithStatusCode(HttpStatusCode.OK);
            };


            // Set color, argument, sensorDrievr for given effect
            Post["/"] = _ =>
            {
                //if (Request.Form.dmxbrightness.HasValue)
                    //effectService.SetArgument((string)Request.Form.argument);

                return HttpStatusCode.OK;
            };
        }
    }
}
