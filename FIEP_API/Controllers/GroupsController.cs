using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DistributedCache;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using BusinessTier.Services;
using DataTier.Models;
using DataTier.UOW;
using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ICacheStore _cacheStore;
        private IUnitOfWork _unitOfWork;
        private NotificationPublisher _notificationPublisher;
        private readonly IMediator _mediator;
        public GroupsController(IUnitOfWork unitOfWork, ICacheStore cacheStore, NotificationPublisher notificationPublisher, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _notificationPublisher = notificationPublisher;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult> GetGroups([FromQuery]GetGroupsRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{GroupId}")]
        public async Task<ActionResult> GetGroupById([FromRoute]GetGroupByIdRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{GroupId:int}/events")]
        public async Task<ActionResult> GetEventsOfGroup([FromRoute]int GroupId,[FromQuery] GetEventsOfGroupRequest request)
        {
            request.SetGroupId(GroupId);
            var result = await _mediator.Send(request);

            return result.Response == null ? BadRequest() : Ok(result.Response);
        }

        [HttpPut("{GroupId}/notification")]
        public async Task<ActionResult> CreatePushNotification([FromRoute] int GroupId, [FromBody] CreateGroupNotificationRequest request)
        {
            request.GroupId = GroupId;
            var result=  await _mediator.Send(request);

            return result.Response == null ? BadRequest() : Ok(result.Response);
        }
    }
}