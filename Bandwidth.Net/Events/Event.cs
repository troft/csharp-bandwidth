using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Bandwidth.Net.Events
{
    public class Event
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = CreateJsonSettings();
        public string EventType { get; set; }
        public string ApplicationId { get; set; }
        public DateTime? Time { get; set; }

        private static JsonSerializerSettings CreateJsonSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.All
            };
            jsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true,
                AllowIntegerValues = false
            });
            jsonSerializerSettings.Converters.Add(new EventCreationConverter());
            jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            return jsonSerializerSettings;
        }

        /// <summary>
        ///     Parse request body text and return assotiated event object
        /// </summary>
        public static Event ParseRequestBody(string body)
        {
            return JsonConvert.DeserializeObject<Event>(body, JsonSerializerSettings);
        }
    }

    internal class EventCreationConverter : JsonCreationConverter<Event>
    {
        protected override Event Create(Type objectType, JObject obj)
        {
            string type = obj.Property("eventType").Value.ToString();
            return Activator.CreateInstance(Type.GetType(GetTypeName(type))) as Event;
        }

        private string GetTypeName(string type)
        {
            if (type == "incomingcall") type = "IncomingCall";
            if (type == "timeout") type = "CallTimeout";
            var buffer = new StringBuilder("Bandwidth.Net.Events.");
            bool useUpperCase = true;
            foreach (char c in type)
            {
                if (useUpperCase)
                {
                    buffer.Append(Char.ToUpper(c));
                    useUpperCase = false;
                }
                else
                {
                    if (c == '-' || c == '_')
                    {
                        useUpperCase = true;
                        continue;
                    }
                    buffer.Append(c);
                }
            }
            return buffer.ToString();
        }
    }

    internal abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject obj);

        public override bool CanConvert(Type objectType)
        {
            return objectType.Namespace == "Bandwidth.Net.Events";
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var target = Create(objectType, obj);
            serializer.Populate(obj.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}