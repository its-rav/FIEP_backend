using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
	public class GetPostsOfEventRequest : IRequest<ResponseBase>
	{
		private int _EventId;
		public void SetEventId(int EventId)
		{
			this._EventId = EventId;

		}
		public int GetEventId() => this._EventId;
		//Paging param
		const int maxPageSize = 10;

		private int _pageSize = 5;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
		public int PageNumber { get; set; } = 1;
		//sort param
		public PostFields Field { get; set; } = 0;
		public Boolean isDesc { get; set; } = true;
		//field param
		private string _FieldSize = "long";
		public string FieldSize
		{
			get
			{
				return _FieldSize;
			}
			set
			{
				_FieldSize = (value.CompareTo("short") == 0 || value.CompareTo("medium") == 0) ? value : "long";
			}
		}
	}
}
