using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    
    public class CreateEventNotificationRequest : IRequest<ResponseBase>
    {
        public int? EventId { get; set; } = null;
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
    }
}
