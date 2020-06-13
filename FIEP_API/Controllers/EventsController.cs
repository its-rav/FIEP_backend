using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using DataTier.Models;
using DataTier.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public EventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult GetEvents([FromQuery]GetEventsRequest request)
        {
            
            var listEventAfterFilter = _unitOfWork.Repository<Event>().GetAll().Where(x => x.IsDeleted == false);
            if (request.SearchParam.Length > 0)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => x.EventName.Contains(request.SearchParam));
            }
            //apply filter
            if (request.ApproveState != 2)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => x.ApprovalState == request.ApproveState);
            }
            if (request.GroupId != 0)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => x.GroupId == request.GroupId);
            }
            if(request.IsUpComming)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => ((DateTime)x.TimeOccur - DateTime.Now).TotalDays < 2 
                                                                    && ((DateTime)x.TimeOccur - DateTime.Now).TotalDays > 0);
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

            var listOfEvents = new List<EventDTO>();
            foreach (var item in listEventsAfterSort)
            {
                EventDTO eventDTO = new EventDTO()
                {
                    EventId = item.EventId,
                    EventName = item.EventName,
                    EventImageUrl = item.ImageUrl,
                    TimeOccur = (DateTime)item.TimeOccur
                };
                listOfEvents.Add(eventDTO);
            }
            return Ok(new {
                data = listOfEvents,
                totalPages = (listEventAfterFilter.ToList().Count/request.PageSize)
            });
        }
        [HttpGet("{id}")]
        public ActionResult GetEvent(int id)
        {
            var result = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == id && x.IsDeleted == false);
            EventDTO eventDTO = new EventDTO()
            {
                EventName = result.EventName,
                EventImageUrl = result.ImageUrl,
                TimeOccur = (DateTime)result.TimeOccur
            };
            return Ok(eventDTO);
        }
    }
}