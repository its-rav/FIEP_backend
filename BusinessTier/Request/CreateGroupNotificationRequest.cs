using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreateGroupNotificationRequest : IRequest<ResponseBase>
    {
        private int _GroupId;
        public int GetGroupId() => this._GroupId;
        public void SetGroupId(int groupId)
        {
            this._GroupId = groupId;
        }

        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; } = null;
    }

}
