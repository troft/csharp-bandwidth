using System;

namespace Bandwidth.Net.Data
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public object Data { get; set; }
    }

    
}