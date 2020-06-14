using System;
using System.Collections.Generic;
using System.Text;

namespace DataTier.Models
{
    public partial class Notification
    {
        public Guid NotificationID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string? UserFCMTokens { get; set; }
        public int? EventId { get; set; }
        public virtual Event? Event { get; set; }
        public int? GroupId { get; set; }
        public virtual GroupInformation? GroupInformation { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}
