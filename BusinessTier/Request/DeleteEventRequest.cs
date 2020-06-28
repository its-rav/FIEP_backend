using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class DeleteEventRequest : IRequest<ResponseBase>
    {
        private int EventId { get; set;}
        public void setEventId(int id)
        {
            EventId = id;
        }
        public int getEventId()
        {
            return EventId;
        }
    }
}
