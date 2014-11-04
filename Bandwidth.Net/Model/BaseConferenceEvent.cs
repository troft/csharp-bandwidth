using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public abstract class BaseConferenceEvent : BaseEvent
    {
        public string ConferenceId { get; set; }
        public string ConferenceUri { get; set; }
        public string Status { get; set; }
    }
}
