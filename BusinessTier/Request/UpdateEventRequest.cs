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
    public class UpdateEventRequest : IRequest<ResponseBase>
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
        public string EventName { get; set; }
        public DateTime? TimeOccur { get; set; } = null;
        public string EventImageUrl { get; set; }
        public int GroupId { get; set; }
        public int? ApprovalState { get; set; } = null;
        public string Location { get; set; }
        public Boolean IsExpired { get; set; } = false;

    }
}
