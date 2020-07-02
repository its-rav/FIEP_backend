using BusinessTier.Response;
using DataTier.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdatePostRequest : UpdateBaseRequest<Post>,IRequest<ResponseBase>
    {
        private Guid PostId { get; set; }
        public void setPostId(Guid id)
        {
            PostId = id;
        }
        public Guid getPostId()
        {
            return PostId;
        }
    }
}
