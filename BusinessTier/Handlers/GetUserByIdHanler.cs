using BusinessTier.DTO;
using BusinessTier.Request;
using BusinessTier.Response;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class GetUserByIdHanler : IRequestHandler<GetUserByIdRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserByIdHanler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {

            var result = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.UserId.Equals(request.UserId) && x.IsDeleted == false);
            if(result == null)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            UserDTO userDTO = new UserDTO()
            {
                UserId = result.UserId,
                FullName = result.Fullname,
                Mail = result.Email,
                AvatarUrl = result.AvatarUrl
            };
            return new ResponseBase()
            {
                Response = userDTO
            };

        }
    }
}
