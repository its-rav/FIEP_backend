using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetEventByIdRequest : IRequest<ResponseBase>
    {
        public int EventId { get; set; }
    }
}
