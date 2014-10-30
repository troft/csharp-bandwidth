using System;

namespace Bandwidth.Net.Model
{
    public class Gather: BaseModel
    {
        public string State { get; set; }
        public string Reason { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public string Call { get; set; }
        public string Digits { get; set; }
    }
  
}