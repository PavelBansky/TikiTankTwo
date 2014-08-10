using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiTankCommon.Patterns;

namespace TikiTankCommon.Converters
{
    public class PatternConverter : CustomCreationConverter<IPattern>
    {
        public override IPattern Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IPattern Create(Type objectType, JObject jObject)
        {
            switch (jObject["Class"].ToString())
            {
                case "TreadEffect":
                    return new TreadEffect();
                case "CameraFlashes":
                    return new CameraFlashes();
                case "Glow":
                    return new Glow();
            }

            throw new ApplicationException(String.Format("Pattern converter: The device type {0} is not supported!", objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object defaultValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject 
            var target = Create(objectType, jObject);

            // Populate the object properties 
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
