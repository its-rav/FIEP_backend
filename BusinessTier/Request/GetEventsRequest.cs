using BusinessTier.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetEventsRequest
    {
		//Search param
		public string SearchParam { get; set; } = "";
		//Filter param
		private int _ApproveState = 1;
		public int GroupId { get; set; } = 0;
		public Boolean IsUpComming { get; set; } = false;
		public int ApproveState
		{
			get
			{
				return _ApproveState;
			}
			set
			{
				_ApproveState = (value >= -1 && value <= 2) ? value : 1;
			}
		}
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
		public EventFields Field { get; set; } = 0;
		public Boolean isDesc { get; set; } = true;
	}
}
