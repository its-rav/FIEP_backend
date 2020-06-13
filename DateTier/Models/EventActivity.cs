using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class EventActivity
    {
        public int ActivityId { get; set; }
        public int EventId { get; set; }
        public int ActivityTypeId { get; set; }
        public string EventActivityDescription { get; set; }
        public Boolean IsDeleted { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public virtual Event Event { get; set; }
    }
}
