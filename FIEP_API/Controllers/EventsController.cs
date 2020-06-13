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
            if (request.Query.Length > 0)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => x.EventName.Contains(request.Query));
            }
            //apply filter
            if (request.ApproveState != 2)
            {
                listEventAfterFilter = listEventAfterFilter.Where(x => x.ApprovalState == request.ApproveState);
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

            var listOfEvents = new List<dynamic>();
            foreach (var item in listEventsAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "s":
                        var eventObj = new
                        {
                            eventID = item.EventId,
                            eventName = item.EventName
                        };

                        listOfEvents.Add(eventObj);
                        break;
                    case "m":
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
            return Ok(new {
                data = listOfEvents,
                totalPages = Math.Ceiling((double)listEventAfterFilter.ToList().Count/request.PageSize)
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

        [HttpGet("{eventID}/posts")]
        public ActionResult GetPostsOfEvent([FromRoute]int eventID,[FromQuery]GetPostsRequest request)
        {
            var listPostsAfterSearch = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.EventId == eventID && x.IsDeleted == false);
            //apply paging
            var listPostsAfterPaging = listPostsAfterSearch
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();

            //apply sort
            var listPostsAfterSort = new List<Post>();
            switch (request.Field)
            {
                case PostFields.CreateDate: //sort by time occur
                    if (request.isDesc)
                    {
                        listPostsAfterSort = listPostsAfterPaging.OrderByDescending(x => x.CreateDate).ToList();
                    }
                    else
                    {
                        listPostsAfterSort = listPostsAfterPaging.OrderBy(x => x.CreateDate).ToList();
                    }
                    break;
            }

            var listOfPosts = new List<dynamic>();
            foreach (var item in listPostsAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "s":
                        var postObj = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent
                        };

                        listOfPosts.Add(postObj);
                        break;
                    case "m":
                        var postObjm = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent,
                            imageUrl = item.ImageUrl,
                            createDate = item.CreateDate
                        };
                        listOfPosts.Add(postObjm);
                        break;
                    default:
                        var postObjl = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent,
                            imageUrl = item.ImageUrl,
                            createDate = item.CreateDate,
                            eventId = item.EventId
                        };

                        listOfPosts.Add(postObjl);
                        break;
                }
            }
            return Ok(new
            {
                data = listOfPosts,
                totalPages = Math.Ceiling((double)listPostsAfterSearch.ToList().Count / request.PageSize)
            });
        }

    }
}