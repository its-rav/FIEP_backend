using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreateCommentRequest : IRequest<ResponseBase>
    {
        public string Content { get; set; }
        public Guid PostID { get; set; }
        public Guid UserId { get; set; }
    }
}
