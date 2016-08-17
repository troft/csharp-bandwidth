using System;
using Newtonsoft.Json;

namespace Bandwidth.Net
{
  internal class StringBooleanConverter : JsonConverter
  {
    public override bool CanWrite => true;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(value?.ToString().ToLowerInvariant());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var value = (reader.Value)?.ToString();
      if (string.CompareOrdinal(value, "true") == 0)
      {
        return true;
      }
      if (string.CompareOrdinal(value, "false") == 0)
      {
        return false;
      }
      return (objectType == typeof (bool?)) ? null : (object) default(bool);
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (bool) || objectType == typeof(bool?);
    }
  }
}
