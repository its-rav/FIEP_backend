using BusinessTier.Response;
using DataTier.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateCommentRequest : IRequest<ResponseBase>
    {
        private Guid CommentId { get; set; }
        public void setCommentId(Guid id)
        {
            CommentId = id;
        }
        public Guid getCommentId()
        {
            return CommentId;
        }
        public string Content { get; set; }
    }
}
