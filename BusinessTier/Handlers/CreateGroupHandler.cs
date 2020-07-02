using BusinessTier.DistributedCache;
using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.Services;
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
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public CreateGroupHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
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
            var flag = _unitOfWork.Commit();
            if (flag != 0 && CachingEnabled)
            {
                _redis.CacheGroupTable();
            }
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
