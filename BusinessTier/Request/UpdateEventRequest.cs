using BusinessTier.DTO;
using BusinessTier.Response;
using DataTier.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateEventRequest : UpdateBaseRequest<Event>,IRequest<ResponseBase>
    {
        private int EventId;
        public int getEventId()
        {
            return EventId;
        }
        public void setEventId(int id)
        {
            EventId = id;
        }
    }
}
