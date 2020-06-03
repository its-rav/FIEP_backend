using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class GroupSubscription
    {
        public int GroupId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
