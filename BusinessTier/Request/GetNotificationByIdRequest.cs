using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetNotificationByIdRequest : IRequest<ResponseBase>
    {
        public Guid NotiID { get; set; }
    }
}
