using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateOrDeleteCommentRequest : IRequest<ResponseBase>
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
        private Boolean IsUpdate { get; set; }
        public void setIsUpdate(Boolean value)
        {
            IsUpdate = value;
        }
        public Boolean getIsUpdate()
        {
            return IsUpdate;
        }
    }
}
