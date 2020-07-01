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
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var existingUser = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.UserId == request.UserId && x.IsDeleted == false);
            if(existingUser == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            existingUser.IsDeleted = true;
            _unitOfWork.Repository<UserInformation>().Update(existingUser);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
