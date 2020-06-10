using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class EventSubscription
    {
        public int EventId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public Boolean IsDeleted { get; set; }
        public virtual Event Event { get; set; }
        public virtual UserInformation User { get; set; }
    }
}
