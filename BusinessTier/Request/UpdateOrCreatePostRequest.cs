using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateOrCreatePostRequest : IRequest<ResponseBase>
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
        public int EventId { get; set; }
        public string PostContent { get; set; }
        public string ImageUrl { get; set; }
        private Boolean isUpdateRequest { get; set; }
        public void setIsUpdate(Boolean value)
        {
            isUpdateRequest = value;
        }
        public Boolean getIsUpdate()
        {
            return isUpdateRequest;
        }
    }
}
