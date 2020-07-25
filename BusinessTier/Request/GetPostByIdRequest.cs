using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetPostByIdRequest : IRequest<ResponseBase>
    {
        public Guid PostId { get; set; }
    }
}
