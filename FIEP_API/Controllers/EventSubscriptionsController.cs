using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.Request;
using DataTier.UOW;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventSubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventSubscriptionsController( IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult> CreateEventSub([FromBody]CreateOrDeleteEventSubRequest request)
        {
            request.setIsCreate(true);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{EventId}/{UserId}")]
        public async Task<ActionResult> DeleteEventSub([FromRoute]CreateOrDeleteEventSubRequest request)
        {
            request.setIsCreate(false);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}