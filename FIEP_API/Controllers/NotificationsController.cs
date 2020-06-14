using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BusinessTier.Request;
using DataTier.UOW;
using DataTier.Models;
using BusinessTier.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: api/<NotificationsController>
        [HttpGet]
        public ActionResult GetNotifications([FromQuery] GetNotificationsRequest request)
        {
            var listNotificationsAfterFilter = _unitOfWork.Repository<Notification>().GetAll();
            if (request.Query.Length > 0)
            {
                listNotificationsAfterFilter = listNotificationsAfterFilter.Where(x => x.Title.Contains(request.Query));
            }
            //apply paging
            var listNotificationssAfterPaging = listNotificationsAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            //apply sort
            return Ok(new
            {
                data = listNotificationssAfterPaging,
                totalPages = Math.Ceiling((double)listNotificationssAfterPaging.ToList().Count / request.PageSize)
            });
        }

        // GET api/<NotificationsController>/5
        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var result = _unitOfWork.Repository<Notification>().FindFirstByProperty(x => x.NotificationID == id);
            NotificationDTO notificationDTO = new NotificationDTO()
            {
                NotificationID = result.NotificationID,
                Title = result.Title,
                Body = result.Body,
                ImageUrl = result.ImageUrl,
                EventId=result.EventId,
                GroupId = result.GroupId,
                UserFCMTokens = result.UserFCMTokens
            };
            return Ok(notificationDTO);
        }

        // POST api/<NotificationsController>
        /* [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] CreateNotificationRequest request)
        {

            _unitOfWork.Repository<Notification>().Insert()

             MulticastMessage multicastMessage = new MulticastMessage()
             {
                 Tokens = request.FCMTokens,
                 Notification = new Notification(){
                     Body = request.Notification.Body,
                     Title = request.Notification.Title,
                     ImageUrl = request.Notification.ImageUrl
                 },
             };

             var messaging = FirebaseMessaging.DefaultInstance;
             var response = await messaging.SendMulticastAsync(multicastMessage);
             var failedTokens = new List<string>();

             if (response.FailureCount > 0)
             {
                 for (var i = 0; i < response.Responses.Count; i++)
                 {
                     if (!response.Responses[i].IsSuccess)
                     {
                         // The order of responses corresponds to the order of the registration tokens.
                         failedTokens.Add(request.FCMTokens[i]);
                     }
                 }

                 Console.WriteLine($"List of tokens that caused failures: {failedTokens}");
             }
             return Ok(failedTokens);
            return new StatusCodeResult(StatusCodes.Status501NotImplemented);
        }*/
        /*
        // PUT api/<NotificationsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);
        }

        // DELETE api/<NotificationsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);
        }*/
    }
}
