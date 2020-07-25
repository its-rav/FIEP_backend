using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreateOrDeleteEventSubRequest : IRequest<ResponseBase>
    {
        public int EventId { get; set; }
        public Guid UserId { get; set; }
        private Boolean isCreateRequest { get; set; }
        public void setIsCreate(Boolean value)
        {
            isCreateRequest = value;
        }
        public Boolean getIsCreate()
        {
            return isCreateRequest;
        }
    }
}
