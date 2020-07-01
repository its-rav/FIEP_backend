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
using System.Web.Http.ModelBinding;

namespace BusinessTier.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            if (request.patchDoc != null)
            {
                var existingUser = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.UserId == request.getUserId() && x.IsDeleted == false);

                if(existingUser == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                request.patchDoc.ApplyTo(existingUser);
                _unitOfWork.Commit();
                return new ResponseBase()
                {
                    Response = 1
                };
            }
            else
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
        }
    }
}
