using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DistributedCache;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.ServiceWorkers;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ICacheStore _cacheStore;
        private readonly IUnitOfWork _unitOfWork;
        private NotificationPublisher _notificationPublisher;
        private readonly IMediator _mediator;
        public EventsController(IUnitOfWork unitOfWork, ICacheStore cacheStore, NotificationPublisher notificationPublisher, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _notificationPublisher = notificationPublisher;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetEvents([FromQuery]GetEventsRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{EventId}")]
        public async Task<ActionResult> GetEvent([FromRoute] GetEventByIdRequest request)
        {
            var result = await _mediator.Send(request);
            if(result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
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
                    case "short":
                        var postObj = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent
                        };

                        listOfPosts.Add(postObj);
                        break;
                    case "medium":
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

        [HttpPut("{eventID}/notification")]
        public async Task<ActionResult> CreatePushNotification([FromRoute] int eventID, [FromBody] CreateNotificationRequest request)
        {
            DataTier.Models.Notification notification = new DataTier.Models.Notification()
            {
                NotificationID = new Guid(),
                Body = request.Body,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                EventId = eventID
            };
            _unitOfWork.Repository<DataTier.Models.Notification>().Insert(notification);

            _unitOfWork.Commit();
            //add to redis
            _notificationPublisher.Publish(notification.NotificationID.ToString());

            return Ok();
        }
        [HttpPost]
        public ActionResult CreateEvent([FromQuery] EventDTO dto)
        {
            var model = new Event()
            {
                EventId = dto.EventId,
                EventName = dto.EventName,
                GroupId = 3,
            };
            _unitOfWork.Repository<Event>().Update(model);
            _unitOfWork.Commit();
            return Ok("testing");
        }
    }
}