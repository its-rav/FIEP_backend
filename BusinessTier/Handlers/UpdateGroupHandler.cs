using BusinessTier.DistributedCache;
using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.Services;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class UpdateGroupHandler : IRequestHandler<UpdateGroupRequest,ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public UpdateGroupHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
        }
        public async Task<ResponseBase> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            if(request.patchDoc != null)
            {
                var existingGroup = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == request.getGroupId() && x.IsDeleted == false);

                if (existingGroup == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                request.patchDoc.ApplyTo(existingGroup);
                var result =_unitOfWork.Commit();
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
