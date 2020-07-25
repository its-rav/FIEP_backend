using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetNotificationsRequest :FilterParametersRequest<NotificationFields>, IRequest <ResponseBase>
    {
		//Search param
		public string Query { get; set; } = "";
		
	}
}
