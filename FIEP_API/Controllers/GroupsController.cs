using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DistributedCache;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using BusinessTier.ServiceWorkers;
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
        public ActionResult GetGroups([FromQuery]GetGroupsRequest request)
        {
            var listGroupAfterFilter = _unitOfWork.Repository<GroupInformation>().GetAll().Where(x => x.IsDeleted == false);
            if (request.Query.Length > 0)
            {
                listGroupAfterFilter = listGroupAfterFilter.Where(x => x.GroupName.Contains(request.Query));
            }
            //apply paging
            var listGroupsAfterPaging = listGroupAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            //apply sort
            var listGroupsAfterSort = new List<GroupInformation>();
            switch (request.Field)
            {
                case GroupFields.Follower: //sort by number of follower
                    if (request.isDesc)
                    {
                        listGroupsAfterSort = listGroupsAfterPaging.OrderByDescending(x => x.GroupSubscription.Count).ToList();
                    }
                    else
                    {
                        listGroupsAfterSort = listGroupsAfterPaging.OrderBy(x => x.GroupSubscription.Count).ToList();
                    }
                    break;
            }

            var listOfGroups = new List<dynamic>();
            foreach (var item in listGroupsAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "short":
                        var groupObj = new
                        {
                            groupID = item.GroupId,
                            groupName = item.GroupName
                        };

                        listOfGroups.Add(groupObj);
                        break;
                    case "medium":
                        var groupObjm = new
                        {
                            groupID = item.GroupId,
                            groupName = item.GroupName,
                            imageUrl = item.GroupImageUrl,
                        };
                        listOfGroups.Add(groupObjm);
                        break;
                    default:
                        var groupObjl = new
                        {
                            groupID = item.GroupId,
                            groupName = item.GroupName,
                            imageUrl = item.GroupImageUrl,
                            manager = item.GroupManagerId,
                        };
                        listOfGroups.Add(groupObjl);
                        break;
                }
            }
            return Ok(new
            {
                data = listOfGroups,
                totalPages = Math.Ceiling((double)listGroupAfterFilter.ToList().Count / request.PageSize)
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetGroupById(int id)
        {
            var group = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == id && x.IsDeleted == false);
            var groupDTO = new GroupDTO()
            {
                GroupName = group.GroupName,
                GroupImageUrl = group.GroupImageUrl,
                GroupFollower = group.GroupSubscription.Count
            };
            return Ok(groupDTO);
        }

        [HttpGet("{groupId:int}/events")]
        public async Task<ActionResult> GetEventsOfGroup([FromRoute]int groupId,[FromQuery] GetEventsRequest request)
        {
            request.GroupId = groupId;
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpPut("{groupId}/notification")]
        public async Task<ActionResult> CreatePushNotification([FromRoute] int groupId, [FromBody] CreateNotificationRequest request)
        {
            DataTier.Models.Notification notification = new DataTier.Models.Notification()
            {
                NotificationID=new Guid(),
                Body = request.Body,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                GroupId= groupId
            };
            _unitOfWork.Repository<DataTier.Models.Notification>().Insert(notification);
            _unitOfWork.Commit();

            _notificationPublisher.Publish(notification.NotificationID.ToString());

            return Ok();
        }
    }
}