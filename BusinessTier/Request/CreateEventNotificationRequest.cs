using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    
    public class CreateEventNotificationRequest : IRequest<ResponseBase>
    {
        private int _EventId;
        public int GetEventId() => this._EventId;
        public void SetEventId(int eventId)
        {
            this._EventId = eventId;
        }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string ImageUrl { get; set; } = null;
    }
}
