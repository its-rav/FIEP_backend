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
            _unitOfWork.Repository<GroupInformation>().Insert(newGroup);
            _unitOfWork.Commit();
            var existingGroup = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupName.Equals(request.GroupName)
                                                                                                && x.GroupImageUrl.Equals(request.ImageUrl) && x.IsDeleted == false);
            var newGroupSub = new GroupSubscription()
            {
                GroupId = existingGroup.GroupId,
                UserId = request.ManagerId,
                SubscriptionType = 2,
            };
            existingGroup.GroupSubscription.Add(newGroupSub);
            _unitOfWork.Repository<GroupInformation>().Update(newGroup);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
