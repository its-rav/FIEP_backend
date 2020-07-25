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

using BusinessTier.Services;
using MediatR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public NotificationsController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        // GET: api/<NotificationsController>
        [HttpGet]
        public async Task<ActionResult> GetNotifications([FromQuery] GetNotificationsRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result.Response);
            
        }

        // GET api/<NotificationsController>/5
        [HttpGet("{NotiID}")]
        public async Task<ActionResult> Get([FromRoute]GetNotificationByIdRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
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
