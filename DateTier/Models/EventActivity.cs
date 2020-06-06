using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class EventActivity
    {
        public int ActivityId { get; set; }
        public int ActivityTypeId { get; set; }
        public string ActivityTypeDescription { get; set; }

        public virtual Event Activity { get; set; }
        public virtual ActivityType ActivityType { get; set; }
    }
}
