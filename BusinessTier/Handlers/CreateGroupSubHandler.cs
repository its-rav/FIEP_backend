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
    public class CreateGroupSubHandler : IRequestHandler<CreateGroupSubRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateGroupSubHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(CreateGroupSubRequest request, CancellationToken cancellationToken)
        {
            var existingGroupSub = _unitOfWork.Repository<GroupSubscription>().FindFirstByProperty(x => x.GroupId == request.GroupId &&
                                                                                                        x.UserId == request.UserId);
            if (request.SubscriptionType != 1 && request.SubscriptionType != 2)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            if (existingGroupSub != null)
            {
                if (existingGroupSub.IsDeleted == true)
                {
                    existingGroupSub.IsDeleted = false;
                }
                existingGroupSub.SubscriptionType = request.SubscriptionType;
                _unitOfWork.Repository<GroupSubscription>().Update(existingGroupSub);
            }
            else if(existingGroupSub == null)
            {               
                var newGroupSub = new GroupSubscription()
                {
                    GroupId = request.GroupId,
                    UserId = request.UserId,
                    SubscriptionType = request.SubscriptionType
                };
                _unitOfWork.Repository<GroupSubscription>().Insert(newGroupSub);
            }
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
