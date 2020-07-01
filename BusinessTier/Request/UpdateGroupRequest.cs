using BusinessTier.Response;
using DataTier.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateGroupRequest : UpdateBaseRequest<GroupInformation>,IRequest<ResponseBase>
    {
        private int Groupid { get; set; }
        public void setGroupId(int id)
        {
            Groupid = id;
        }
        public int getGroupId()
        {
            return Groupid;
        }
    }
}
