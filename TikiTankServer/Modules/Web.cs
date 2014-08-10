using Nancy;
using System;
using System.Collections.Generic;

namespace TikiTankServer.Modules
{
    public class Web : NancyModule
    {
        public Web()
        {
            Get["/"] = _ =>
            {
                return View["index.sshtml"];
            };

            Get["/treads"] = _ =>
            {
                List<string> myList = new List<string>() { "Pavel", "Honza", "John" };

                return View["treads.sshtml", myList.ToArray()];
            };
        }
    }
}
