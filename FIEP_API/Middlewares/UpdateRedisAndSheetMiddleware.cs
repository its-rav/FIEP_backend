using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DistributedCache;
using BusinessTier.Services;
using DataTier.Models;
using DataTier.UOW;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FIEP_API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UpdateRedisAndSheetMiddleware
    {
        private readonly RequestDelegate _next;

        const string eventCacheKey = "EventsTable";
        const string groupCacheKey = "GroupsTable";
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private readonly IUnitOfWork _unitOfWork;
        private bool CachingEnabled => _cachingEnabled;

        public UpdateRedisAndSheetMiddleware(RequestDelegate next, ICacheStore cacheStore, IUnitOfWork unitOfWork)
        {
            _next = next;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _unitOfWork = unitOfWork;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //cache event table data
            if(!_cacheStore.IsExist(eventCacheKey))
            {
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
            //cache group table data
            if (!_cacheStore.IsExist(groupCacheKey))
            {
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

            await _next(httpContext);

            if (!httpContext.Request.Method.Equals("GET") && httpContext.Response.StatusCode == 200)
            {
                //update sheet data after update, insert or delete
                new GoogleSheetApiUtils(_unitOfWork).UpdateDataToSheet();
                var path = httpContext.Request.Path.ToString();
                if(path.Contains("Events") || path.Contains("EventSubscription"))
                {
                    _cacheStore.Remove(eventCacheKey);
                }
                if (path.Contains("Groups") || path.Contains("GroupSubscription"))
                {
                    _cacheStore.Remove(groupCacheKey);
                }
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UpdateRedisAndSheetMiddlewareExtensions
    {
        public static IApplicationBuilder UseUpdateRedisAndSheetMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UpdateRedisAndSheetMiddleware>();
        }
    }
}
