using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetCommentsOfPostRequest: FilterParametersRequest<CommentFields>,IRequest<ResponseBase>
    {
        private Guid PostId { get; set; }
        public void setPostID(Guid id)
        {
            PostId = id;
        }
        public Guid getPostID()
        {
            return PostId;
        }
	}
}
