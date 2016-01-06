using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace NewsDotNet.WebUI.Areas.Admin
{
    public class CustomDateTimeConverter: DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}