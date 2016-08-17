using System;
using Newtonsoft.Json;

namespace Bandwidth.Net
{
  internal class StringBooleanConverter : JsonConverter
  {
    public override bool CanWrite => true;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(value as string);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var value = (reader.Value)?.ToString();
      return string.CompareOrdinal(value, "true") == 0;
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (string) || objectType == typeof (bool) || objectType == typeof(bool?);
    }
  }
}
