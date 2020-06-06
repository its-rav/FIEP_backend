using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class Event
    {
        public Event()
        {
            EventSubscription = new HashSet<EventSubscription>();
            Post = new HashSet<Post>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public int ActivityId { get; set; }
        public int GroupId { get; set; }
        public string Location { get; set; }
        public bool? IsExpired { get; set; }
        public int? ApprovalState { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? TimeOccur { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual GroupInformation Group { get; set; }
        public virtual ICollection<EventSubscription> EventSubscription { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}
