using BusinessTier.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetGroupsRequest
    {
		//Search param
		public string Query { get; set; } = "";
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
		public GroupFields Field { get; set; } = 0;
		public Boolean isDesc { get; set; } = true;
		//field param
		private string _FieldSize = "l";
		public string FieldSize
		{
			get
			{
				return _FieldSize;
			}
			set
			{
				_FieldSize = (value.CompareTo("s") == 0 || value.CompareTo("m") == 0) ? value : "l";
			}
		}

	}
}
