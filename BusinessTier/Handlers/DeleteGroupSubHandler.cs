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
    public class DeleteGroupSubHandler : IRequestHandler<DeleteGroupSubRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGroupSubHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(DeleteGroupSubRequest request, CancellationToken cancellationToken)
        {
            var existingGroupSub = _unitOfWork.Repository<GroupSubscription>().FindFirstByProperty(x => x.GroupId == request.GroupId &&
                                                                                                        x.UserId == request.UserId && x.IsDeleted == false);
            if(existingGroupSub == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            existingGroupSub.IsDeleted = true;
            _unitOfWork.Repository<GroupSubscription>().Update(existingGroupSub);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
