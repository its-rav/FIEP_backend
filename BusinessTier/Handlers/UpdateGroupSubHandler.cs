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
    public class UpdateGroupSubHandler : IRequestHandler<UpdateGroupSubRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateGroupSubHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(UpdateGroupSubRequest request, CancellationToken cancellationToken)
        {
            if (request.patchDoc != null)
            {
                var existingGroupSub = _unitOfWork.Repository<GroupSubscription>().FindFirstByProperty(x => x.GroupId == request.getGroupId() 
                                                                                                        && x.UserId == request.getUserId() && x.IsDeleted == false);

                if (existingGroupSub == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                request.patchDoc.ApplyTo(existingGroupSub);
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
