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
            Notification = new HashSet<Notification>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public int GroupId { get; set; }
        public string Location { get; set; }
        public int? ApprovalState { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? TimeOccur { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public Boolean IsExpired { get; set; }
        public Boolean IsDeleted { get; set; }
        public virtual GroupInformation Group { get; set; }
        public virtual ICollection<EventSubscription> EventSubscription { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
    }
}
