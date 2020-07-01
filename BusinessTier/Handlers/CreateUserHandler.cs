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
    public class CreateUserHandler : IRequestHandler<CreateUserRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            Boolean flag = false;
            Guid? newUserId = null;
            while(flag == false)
            {
                newUserId = Guid.NewGuid();
                var existingUser = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.UserId == newUserId);
                if(existingUser == null)
                {
                    flag = true;
                }
            }
            var newUser = new UserInformation()
            {
                UserId = (Guid)newUserId,
                RoleId = request.RoleId,
                Email = request.Email,
                Fullname = request.FullName,
                AvatarUrl = request.AvatarUrl
            };
            _unitOfWork.Repository<UserInformation>().Insert(newUser);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
