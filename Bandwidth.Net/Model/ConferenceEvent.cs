using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class ConferenceEvent : BaseConferenceEvent
    {
        public DateTime CreatedTime { get; set; }
        public DateTime CompletedTime { get; set; }
    }
}
