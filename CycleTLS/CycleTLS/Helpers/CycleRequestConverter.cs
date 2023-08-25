using CycleTLS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CycleTLS.Helpers
{
    public class CycleRequestConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            CycleRequest req = (CycleRequest)value;
            JToken t = JToken.FromObject(value);
            JObject o = (JObject)t;

            o["Options"]["Method"] = req.Options.Method.ToString();

            o.WriteTo(writer);
        }
    }
}
