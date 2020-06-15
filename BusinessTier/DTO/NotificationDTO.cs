using System;

namespace BusinessTier.DTO
{
    public class NotificationDTO
    {
        public Guid NotificationID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public int? EventId { get; set; }
        public int? GroupId { get; set; }
        public string? UserFCMTokens { get; set; }
    }
}