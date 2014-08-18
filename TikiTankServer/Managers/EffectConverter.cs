using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using TikiTankCommon;
using TikiTankCommon.Effects;

namespace TikiTankServer.Managers
{
    public class EffectConverter : CustomCreationConverter<IEffect>
    {
        public override IEffect Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IEffect Create(Type objectType, JObject jsonObject)
        {
            switch (jsonObject["Class"].ToString())
            {
                case "AntialiasTread":
                    return new AntialiasTread();
                case "AntsTread":
                    return new AntsTread();
                case "CameraFlashes":
                    return new CameraFlashes();
                case "DMXGlow":
                    return new DMXGlow();
                case "DMXRainbow":
                    return new DMXRainbow();
                case "DMXSolidColor":
                    return new DMXSolidColor();
                case "FullRainbowTread":
                    return new FullRainbowTread();
                case "Glow":
                    return new Glow();
                case "Gradient":
                    return new Gradient();
                case "KitBarrel":
                    return new KitBarrel();
                case "MultiLineBarrel":
                    return new MultiLineBarrel();
                case "Rainbow":
                    return new Rainbow();
                case "RainbowTread":
                    return new RainbowTread();
                case "RandomCameras":
                    return new RandomCameras();
                case "SimpleTread":
                    return new SimpleTread();
                case "SolidColor":
                    return new SolidColor();
                case "ShootingStar":
                    return new ShootingStar();
                case "SpotTread":
                    return new SpotTread();
                case "SparkleFlashes":
                    return new SparkleFlashes();
                case "TwinkleTwinkle":
                    return new TwinkleTwinkle();
            }

            throw new ApplicationException(String.Format("The effect type {0} is not supported!", objectType));
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
