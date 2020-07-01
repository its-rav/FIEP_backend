using BusinessTier.Response;
using DataTier.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateUserRequest : UpdateBaseRequest<UserInformation>,IRequest<ResponseBase>
    {
        private Guid UserId { get; set; }
        public void setUserId (Guid id)
        {
            UserId = id;
        }
        public Guid getUserId()
        {
            return UserId;
        }
    }
}
