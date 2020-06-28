using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreateGroupSubRequest : IRequest<ResponseBase>
    {
        public int GroupId { get; set; }
        public Guid UserId { get; set; }
        public int SubscriptionType { get; set; }
    }
}
