using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class UserInformation
    {
        public UserInformation()
        {
            Comment = new HashSet<Comment>();
            EventSubscription = new HashSet<EventSubscription>();
            GroupInformation = new HashSet<GroupInformation>();
            GroupSubscription = new HashSet<GroupSubscription>();
        }

        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public bool? IsDeleted { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<EventSubscription> EventSubscription { get; set; }
        public virtual ICollection<GroupInformation> GroupInformation { get; set; }
        public virtual ICollection<GroupSubscription> GroupSubscription { get; set; }
    }
}
