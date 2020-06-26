using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
	public class GetPostsOfEventRequest : FilterParametersRequest<PostFields>, IRequest<ResponseBase>
	{
		private int _EventId;
		public void SetEventId(int EventId)
		{
			this._EventId = EventId;

		}
		public int GetEventId() => this._EventId;
		
	}
}
