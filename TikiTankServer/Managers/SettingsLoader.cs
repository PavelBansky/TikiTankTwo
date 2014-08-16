using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TikiTankCommon;

namespace TikiTankServer.Managers
{
    public class SettingsLoader
    {
        public static void LoadEffects(string fileName, LEDStrip displayStrip, List<EffectContainer> list)
        {
            Console.WriteLine("Settings: Openning file {0}", fileName);
            using (StreamReader rdr = new StreamReader(fileName))
            {
                var effList = JsonConvert.DeserializeObject<List<EffectContainer>>(rdr.ReadToEnd(), new EffectConverter());
                foreach (EffectContainer cont in effList)
                {
                    Console.WriteLine(cont.Information.Name);
                    cont.AssignStrip(displayStrip);
                    list.Add(cont);
                }
            }
        }
    }
}
