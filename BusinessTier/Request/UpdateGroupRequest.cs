using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateOrCreateGroupRequest : IRequest<ResponseBase>
    {
        private int Groupid { get; set; }
        public void setGroupId(int id)
        {
            Groupid = id;
        }
        public int getGroupId()
        {
            return Groupid;
        }
        public string ImageUrl { get; set; }
        public string GroupName { get; set; }
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
