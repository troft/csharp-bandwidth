using System;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Event data
    /// </summary>
    public class Event: BaseModel
    {
        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Event time
        /// </summary>
        public DateTime Time { get; set; }
        
        /// <summary>
        /// Additional data of event
        /// </summary>
        public object Data { get; set; }
    }
}