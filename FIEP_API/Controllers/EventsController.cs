using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DTO;
using BusinessTier.Response;
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
        public ActionResult GetEvents()
        {
            var events = _unitOfWork.Repository<Event>().FindAllByProperty(x => x.IsExpired == false && x.ApprovalState == 1);
            var fillteredByTimeOccurEvent = events.Where(x => ((DateTime)x.TimeOccur - DateTime.Now).TotalDays < 2).ToList();
            var listOfUpCommingEvents = new List<EventDTO>();
            foreach (var item in fillteredByTimeOccurEvent)
            {
                EventDTO eventDTO = new EventDTO()
                {
                    EventName = item.EventName,
                    EventImageUrl = item.ImageUrl,
                    TimeOccur = (DateTime)item.TimeOccur
                };
                listOfUpCommingEvents.Add(eventDTO);
            }
            return Ok(new ResponseBase<List<EventDTO>>() {
                ErrorCode = 0,
                Data = listOfUpCommingEvents
            });
        }
    }
}