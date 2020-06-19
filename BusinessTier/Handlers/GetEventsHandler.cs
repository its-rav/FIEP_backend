﻿using BusinessTier.DistributedCache;
using BusinessTier.Fields;
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
    public class GetEventsHandler : IRequestHandler<GetEventsRequest,ResponseBase>
    {
        private readonly ICacheStore _cacheStore;
        private IUnitOfWork _unitOfWork;
        public GetEventsHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }

        public async Task<ResponseBase> Handle(GetEventsRequest request, CancellationToken cancellationToken)
        {
            IQueryable<Event> listEventAfterFilter;
            if (request.GroupId != null)
            {
                 listEventAfterFilter = _unitOfWork.Repository<Event>().GetAll().Where(x => x.GroupId == request.GroupId && x.IsDeleted == false);
                if (listEventAfterFilter.ToList().Count <= 0)
                {
                    return new ResponseBase()
                    {
                        Response = null
                    };
                }
            }
            else
            {
                listEventAfterFilter = _unitOfWork.Repository<Event>().GetAll().Where(x => x.IsDeleted == false);
            }
            if (request.Query.Length > 0)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => x.EventName.Contains(request.Query));
            }
            //apply filter
            if (request.ApproveState != 2)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => x.ApprovalState == request.ApproveState);
            }
            if (request.IsUpComming)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => (DateTime)x.TimeOccur >= DateTime.Now);
            }
            //apply paging
            var listEventsAfterPaging = listEventAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            //apply sort
            var listEventsAfterSort = new List<Event>();
            switch (request.Field)
            {
                case EventFields.TimeOccur: //sort by time occur
                    if (request.isDesc)
                    {
                        listEventsAfterSort = listEventsAfterPaging.OrderByDescending(x => x.TimeOccur).ToList();
                    }
                    else
                    {
                        listEventsAfterSort = listEventsAfterPaging.OrderBy(x => x.TimeOccur).ToList();
                    }
                    break;
                case EventFields.Follower: //sort by number of follower
                    if (request.isDesc)
                    {
                        listEventsAfterSort = listEventsAfterPaging.OrderByDescending(x => x.EventSubscription.Count).ToList();
                    }
                    else
                    {
                        listEventsAfterSort = listEventsAfterPaging.OrderBy(x => x.EventSubscription.Count).ToList();
                    }
                    break;
            }

            var listOfEvents = new List<dynamic>();
            foreach (var item in listEventsAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "short":
                        var eventObj = new
                        {
                            eventID = item.EventId,
                            eventName = item.EventName
                        };

                        listOfEvents.Add(eventObj);
                        break;
                    case "medium":
                        var eventObjm = new
                        {
                            eventID = item.EventId,
                            eventName = item.EventName,
                            imageUrl = item.ImageUrl,
                            timeOccur = item.TimeOccur,
                            location = item.Location
                        };
                        listOfEvents.Add(eventObjm);
                        break;
                    default:
                        var eventObjl = new
                        {
                            eventID = item.EventId,
                            eventName = item.EventName,
                            imageUrl = item.ImageUrl,
                            timeOccur = item.TimeOccur,
                            location = item.Location,
                            groupID = item.GroupId,
                            createDate = item.CreateDate,
                            approveState = item.ApprovalState
                        };

                        listOfEvents.Add(eventObjl);
                        break;
                }
            }
            var response = new
            {
                data = listOfEvents,
                totalPages = Math.Ceiling((double)listEventAfterFilter.ToList().Count / request.PageSize)
            };
            return new ResponseBase()
            {
                Response = response
            };
        }
    }
}