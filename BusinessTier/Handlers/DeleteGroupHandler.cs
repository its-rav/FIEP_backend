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
    public class DeleteGroupHandler : IRequestHandler<DeleteGroupRequest,ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public DeleteGroupHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
        }
        public async Task<ResponseBase> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
        {
            var existingGroup = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == request.GroupId && x.IsDeleted == false);
            if (existingGroup == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            existingGroup.IsDeleted = true;
            _unitOfWork.Repository<GroupInformation>().Update(existingGroup);
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
    }
}
