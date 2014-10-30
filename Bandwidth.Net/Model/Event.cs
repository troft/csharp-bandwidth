using System;

namespace Bandwidth.Net.Model
{
    public class Event: BaseModel
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public object Data { get; set; }
    }
}