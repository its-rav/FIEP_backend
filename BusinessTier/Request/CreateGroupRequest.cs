using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreateGroupRequest : IRequest<ResponseBase>
    {
        public string ImageUrl { get; set; }
        public string GroupName { get; set; }
        public Guid ManagerId { get; set; }
    }
}
