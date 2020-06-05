using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class EventDTO
    {
        public string EventName { get; set; }
        public DateTime TimeOccur { get; set; }

        public string EventImageUrl { get; set; }
    }
}
