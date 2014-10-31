using System;
using System.Collections.Generic;
using Bandwidth.Net.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bandwidth.Net
{
    public abstract class BaseEvent: BaseModel
    {
        public static BaseEvent CreateFromString(string json)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true,
                AllowIntegerValues = false
            });
            jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            var obj = JsonConvert.DeserializeAnonymousType(json, new { EventType = "" }, jsonSerializerSettings);
            Type type;
            if (EventTypes.TryGetValue(obj.EventType, out type))
            {
                return JsonConvert.DeserializeObject(json, type, jsonSerializerSettings) as BaseEvent;
            }
            throw new NotSupportedException();
        }

        private static readonly Dictionary<string, Type> EventTypes = new Dictionary<string, Type>
        {
            {"incomingcall", typeof(IncomingCallEvent)}
            //TODO fill with another events
        };

        public DateTime Time { get; set; }
        public string Tag { get; set; }
        public string EventType { get; set; }

    }
}
