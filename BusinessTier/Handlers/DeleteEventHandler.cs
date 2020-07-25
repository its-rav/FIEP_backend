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
    public class DeleteEventHandler : IRequestHandler<DeleteEventRequest, ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public DeleteEventHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
        }
        public async Task<ResponseBase> Handle(DeleteEventRequest request, CancellationToken cancellationToken)
        {
            var existingEvent = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == request.EventId && x.IsDeleted == false);
            if (existingEvent == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            existingEvent.IsDeleted = true;
            _unitOfWork.Repository<Event>().Update(existingEvent);
            var result = _unitOfWork.Commit();
            if (result != 0 && CachingEnabled)
            {
                _redis.CacheEventTable();
            }
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
