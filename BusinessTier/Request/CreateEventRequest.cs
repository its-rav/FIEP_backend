using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreateEventRequest : IRequest<ResponseBase>
    {
        public string EventName { get; set; }
        public DateTime TimeOccur { get; set; }
        public string EventImageUrl { get; set; }
        public int GroupId { get; set; }
        public string Location { get; set; }
    }
}
