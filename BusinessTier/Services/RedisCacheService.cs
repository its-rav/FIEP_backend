using BusinessTier.DistributedCache;
using DataTier.Models;
using DataTier.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessTier.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        const string eventCacheKey = "EventsTable";
        const string groupCacheKey = "GroupsTable";
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private readonly IUnitOfWork _unitOfWork;
        private bool CachingEnabled => _cachingEnabled;

        public RedisCacheService(ICacheStore cacheStore, IUnitOfWork unitOfWork)
        {
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _unitOfWork = unitOfWork;
        }
        public void CacheEventTable()
        {
            if (CachingEnabled)
            {
                if(_cacheStore.IsExist(eventCacheKey))
                {
                    _cacheStore.Remove(eventCacheKey);
                }
                List<Event> events = new List<Event>();
                List<EventSubscription> subs;
                foreach (var item in _unitOfWork.Repository<Event>().GetAll())
                {
                    subs = new List<EventSubscription>();
                    var tmp = new Event()
                    {
                        EventName = item.EventName,
                        EventId = item.EventId,
                        ApprovalState = item.ApprovalState,
                        GroupId = item.GroupId,
                        Location = item.Location,
                        TimeOccur = item.TimeOccur,
                        CreateDate = item.CreateDate,
                        ModifyDate = item.ModifyDate,
                        IsExpired = item.IsExpired,
                        IsDeleted = item.IsDeleted,
                    };
                    foreach (var eventSub in item.EventSubscription)
                    {
                        EventSubscription eventSubscription = new EventSubscription()
                        {
                            UserId = eventSub.UserId
                        };
                        subs.Add(eventSubscription);
                    }
                    tmp.EventSubscription = subs;
                    events.Add(tmp);
                }
                IQueryable<Event> eventTable = events.Select(x => x).AsQueryable<Event>();
                _cacheStore.Add(eventCacheKey, events);
            }
        }

        public void CacheGroupTable()
        {
            if (CachingEnabled)
            {
                if (_cacheStore.IsExist(groupCacheKey))
                {
                    _cacheStore.Remove(groupCacheKey);
                }
                List<GroupInformation> groups = new List<GroupInformation>();
                List<GroupSubscription> subs;
                foreach (var item in _unitOfWork.Repository<GroupInformation>().GetAll())
                {
                    subs = new List<GroupSubscription>();
                    var tmp = new GroupInformation()
                    {
                        GroupName = item.GroupName,
                        GroupImageUrl = item.GroupImageUrl,
                        GroupId = item.GroupId,
                        CreateDate = item.CreateDate,
                        ModifyDate = item.ModifyDate,
                        IsDeleted = item.IsDeleted,
                    };
                    foreach (var eventSub in item.GroupSubscription)
                    {
                        GroupSubscription groupSubscription = new GroupSubscription()
                        {
                            UserId = eventSub.UserId
                        };
                        subs.Add(groupSubscription);
                    }
                    tmp.GroupSubscription = subs;
                    groups.Add(tmp);
                }
                IQueryable<GroupInformation> eventTable = groups.Select(x => x).AsQueryable<GroupInformation>();
                _cacheStore.Add(groupCacheKey, groups);
            }
        }
    }
}
