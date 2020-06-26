using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetEventsRequest : FilterParametersRequest<EventFields>, IRequest<ResponseBase>
    {
		//Search param
		public string Query { get; set; } = "";
		public Boolean IsUpComming { get; set; } = false;
		
	}
}
