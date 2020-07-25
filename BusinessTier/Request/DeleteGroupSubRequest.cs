using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class DeleteGroupSubRequest :IRequest<ResponseBase>
    {
        public int GroupId { get; set; }
        public Guid UserId { get; set; }
    }
}
