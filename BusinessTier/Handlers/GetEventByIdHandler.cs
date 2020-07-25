using BusinessTier.DistributedCache;
using BusinessTier.DTO;
using BusinessTier.Request;
using BusinessTier.Response;
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
    public class GetEventByIdHandler : IRequestHandler<GetEventByIdRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        const string eventCacheKey = "EventsTable";
        public GetEventByIdHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }
        public async Task<ResponseBase> Handle(GetEventByIdRequest request, CancellationToken cancellationToken)
        {
            Event result = null;
            if (_cacheStore.IsExist(eventCacheKey))
            {
                foreach (var item in _cacheStore.Get<List<Event>>(eventCacheKey))
                {
                    if (item.EventId == request.EventId && item.IsDeleted == false)
                    {
                        result = item;
                        break;
                    }
                }
            }
            else
            {
                result = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == request.EventId && x.IsDeleted == false);
            }
            if (result == null)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            EventDTO eventDTO = new EventDTO()
            {
                EventId = result.EventId,
                EventName = result.EventName,
                EventImageUrl = result.ImageUrl,
                Follower = result.EventSubscription.Where(x => x.IsDeleted == false).ToList().Count,
                TimeOccur = (DateTime)result.TimeOccur,
                Group = new GroupDTO()
                {
                    GroupId = result.GroupId,
                    GroupImageUrl = result.Group.GroupImageUrl,
                    GroupName = result.Group.GroupName,
                }
            };
            return new ResponseBase()
            {
                Response = eventDTO
            };
        }
    }
}
