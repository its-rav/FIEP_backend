using BusinessTier.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreateNotificationRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
    }

}
