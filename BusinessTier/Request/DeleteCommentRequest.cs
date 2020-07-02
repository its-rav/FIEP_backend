using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class DeleteCommentRequest : IRequest<ResponseBase>
    {
        public Guid CommentId { get; set; }
    }
}
