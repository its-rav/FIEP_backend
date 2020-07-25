using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class DeleteGroupRequest : IRequest<ResponseBase>
    {
        public int GroupId { get; set; }
    }
}
