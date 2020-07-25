using BusinessTier.DistributedCache;
using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.Services;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventRequest, ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public UpdateEventHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
        }
        public async Task<ResponseBase> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            var newEvent = new Event()
            {
                EventName = request.EventName,
                ImageUrl = request.EventImageUrl,
                ApprovalState = request.ApprovalState,
                Location = request.Location,
                TimeOccur = request.TimeOccur,
                GroupId = request.GroupId,
                IsExpired = request.IsExpired
            };
            CopyValues<Event>(patchDoc, newEvent);
            if (patchDoc != null)
            {
                var existingEvent = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == request.getEventId() && x.IsDeleted == false);

                if (existingEvent == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                patchDoc.ApplyTo(existingEvent);
                int result = _unitOfWork.Commit();
                if(result != 0 && CachingEnabled)
                {
                    _redis.CacheEventTable();
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

        public void CopyValues<T>(JsonPatchDocument patchDoc, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null && !prop.Name.Contains("EventId"))
                {
                    if (prop.Name.Contains("GroupId") && (int)prop.GetValue(source, null) != 0)
                    {
                        patchDoc.Replace(prop.Name, value);
                    }
                    else if (!prop.Name.Contains("GroupId"))
                    {
                        patchDoc.Replace(prop.Name, value);
                    }
                }
            }
        }

    }
}
