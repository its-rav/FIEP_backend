using BusinessTier.Response;
using DataTier.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateGroupSubRequest : IRequest<ResponseBase>
    {
        private int GroupId { get; set; }
        private Guid UserId { get; set; }
        public void setGroupId(int id)
        {
            GroupId = id;
        }
        public int getGroupId()
        {
            return GroupId;
        }
        public void setUserId(Guid id)
        {
            UserId = id;
        }
        public Guid getUserId()
        {
            return UserId;
        }
        public int SubscriptionType { get; set; }
    }
}
