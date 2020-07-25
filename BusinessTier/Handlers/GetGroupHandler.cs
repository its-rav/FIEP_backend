using BusinessTier.DistributedCache;
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
    public class GetGroupHandler : IRequestHandler<GetGroupByIdRequest, ResponseBase>
    {
        private readonly ICacheStore _cacheStore;
        private IUnitOfWork _unitOfWork;
        const string groupCacheKey = "GroupsTable";
        public GetGroupHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }
        public async Task<ResponseBase> Handle(GetGroupByIdRequest request, CancellationToken cancellationToken)
        {
            GroupInformation group= null;
            if(_cacheStore.IsExist(groupCacheKey))
            {
                foreach(var item in _cacheStore.Get<List<GroupInformation>>(groupCacheKey))
                {
                    if(item.GroupId == request.GroupId && item.IsDeleted == false)
                    {
                        group = item;
                        break;
                    }
                }
            }
            else
            {
                group = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == request.GroupId && x.IsDeleted == false);
            }
            if (group == null)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            var groupDTO = new GroupDTO()
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                GroupImageUrl = group.GroupImageUrl,
                GroupFollower = group.GroupSubscription.Count,
            };
            return new ResponseBase()
            {
                Response = groupDTO
            };
        }
    }
}
