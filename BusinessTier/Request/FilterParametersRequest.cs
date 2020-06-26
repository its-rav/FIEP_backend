using System;

namespace BusinessTier.Request
{
    public abstract class FilterParametersRequest<T> where T: Enum
	{
		//Filter param
		private int _ApproveState = 1;
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

		public Boolean isDesc { get; set; } = true;
        //field param
		public T SortBy { get; set; } = (T)Enum.ToObject(typeof(T), 0);

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
