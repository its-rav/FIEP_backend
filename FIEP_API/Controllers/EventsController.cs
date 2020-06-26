using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DistributedCache;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.Services;
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
        public async Task<ActionResult> GetPostsOfEvent([FromRoute]int eventID,[FromQuery]GetPostsOfEventRequest request)
        {
            request.SetEventId(eventID);
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpPut("{eventID}/notification")]
        public async Task<ActionResult> CreatePushNotification([FromRoute] int eventID, [FromBody] CreateEventNotificationRequest request)
        {
            request.SetEventId(eventID);
            await _mediator.Send(request);
            return Ok();
        }
        [HttpPost]
        public ActionResult CreateEvent()
        {
            var model = new Event()
            {
                EventId = 2,
                EventName = "Tiktok competition",
                GroupId = 3,
                Location = "F-Tech Tower",
                ApprovalState = 1,
                ImageUrl = "https://firebasestorage.googleapis.com/v0/b/fiep-e6602.appspot.com/o/event-tiktok-intrument.jpg?alt=media&token=3eec5742-57c7-429b-9832-4c1574d25969",
            };
            _unitOfWork.Repository<Event>().Update(model);
            _unitOfWork.Commit();
            return Ok("testing");
        }
    }
}