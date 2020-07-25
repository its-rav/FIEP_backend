using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
   public  class GetEventsOfGroupRequest: FilterParametersRequest<EventFields>,IRequest<ResponseBase>
	{
		private int _GroupId;
		public int GetGroupId()
        {
			return _GroupId;
        }
		public void SetGroupId(int GroupId)
        {
			this._GroupId = GroupId;
        }
		//Search param
		public string Query { get; set; } = "";
		
		public Boolean IsUpComming { get; set; } = false;
		
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
	}
}
