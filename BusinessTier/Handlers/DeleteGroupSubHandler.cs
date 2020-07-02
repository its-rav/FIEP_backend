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
    public class DeleteGroupSubHandler : IRequestHandler<DeleteGroupSubRequest,ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public DeleteGroupSubHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
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
