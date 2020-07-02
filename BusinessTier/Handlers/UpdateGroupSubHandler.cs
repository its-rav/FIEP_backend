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
    public class UpdateGroupSubHandler : IRequestHandler<UpdateGroupSubRequest,ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public UpdateGroupSubHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
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
                var result =  _unitOfWork.Commit();
                if (result != 0 && CachingEnabled)
                {
                    _redis.CacheGroupTable();
                }
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
