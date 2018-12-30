using Newtonsoft.Json;
using System;

namespace Ademund.Utils
{
    public class AbstractConverter<TReal, TAbstract> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(TAbstract);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => serializer.Deserialize<TReal>(reader);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => serializer.Serialize(writer, value);
    }
}