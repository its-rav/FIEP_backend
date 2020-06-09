using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTier.Models;
using DataTier.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventSubscriptionsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public EventSubscriptionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public ActionResult GetFollowingEventsOfUser([FromQuery] Guid UserID)
        {
            var result = _unitOfWork.Repository<EventSubscription>().FindAllByProperty(x => x.UserId.Equals(UserID));
            return Ok(result);
        }
    }
}