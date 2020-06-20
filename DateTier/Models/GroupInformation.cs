using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class GroupInformation
    {
        public GroupInformation()
        {
            Event = new HashSet<Event>();
            GroupSubscription = new HashSet<GroupSubscription>();
            Notification = new HashSet<Notification>();
        }

        public int GroupId { get; set; }
        public string GroupImageUrl { get; set; }
        public string GroupName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public Boolean IsDeleted { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<GroupSubscription> GroupSubscription { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
    }
}
