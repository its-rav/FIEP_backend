using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupSubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GroupSubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPatch("{GroupId}/{UserId}")]
        public async Task<ActionResult> UpdateGroupSub([FromRoute]int GroupId, [FromRoute]Guid UserId,[FromBody]UpdateGroupSubRequest request)
        {
            request.setGroupId(GroupId);
            request.setUserId(UserId);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> CreateGroupSub([FromBody]CreateGroupSubRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{GroupId}/{UserId}")]
        public async Task<ActionResult> DeleteGroupSub([FromRoute]DeleteGroupSubRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}