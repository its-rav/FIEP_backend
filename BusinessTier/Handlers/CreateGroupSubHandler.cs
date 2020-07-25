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
    public class CreateGroupSubHandler : IRequestHandler<CreateGroupSubRequest,ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public CreateGroupSubHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
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
            var result = _unitOfWork.Commit();
            if (result != 0 && CachingEnabled)
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
