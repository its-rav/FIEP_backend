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
    public class CreateGroupHandler : IRequestHandler<CreateGroupRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateGroupHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            var newGroup = new GroupInformation()
            {
                GroupName = request.GroupName,
                GroupImageUrl = request.ImageUrl,
                
            };
            var result = _unitOfWork.Repository<GroupInformation>().Insert(newGroup);
            _unitOfWork.Commit();
            var id = result.Entity.GroupId;
            var newGroupSub = new GroupSubscription()
            {
                GroupId = id,
                UserId = request.ManagerId,
                SubscriptionType = 2,
            };
            _unitOfWork.Repository<GroupSubscription>().Insert(newGroupSub);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
