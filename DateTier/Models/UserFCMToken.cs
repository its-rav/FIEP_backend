using System;
using System.Collections.Generic;
using System.Text;

namespace DataTier.Models
{
    public partial class UserFCMToken
    {
        public int UserFCMId { get; set; }
        public Guid UserID { get; set; }
        public string FCMToken { get; set; }
        public virtual UserInformation User { get; set; }
    }
}
