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
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPut("{CommentID}")]
        public async Task<ActionResult> UpdateComment([FromRoute]Guid CommentID,[FromBody]UpdateOrDeleteCommentRequest request)
        {
            request.setCommentId(CommentID);
            request.setIsUpdate(true);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{CommentID}")]
        public async Task<ActionResult> DeleteComment([FromRoute]Guid CommentID)
        {
            UpdateOrDeleteCommentRequest request = new UpdateOrDeleteCommentRequest();
            request.setCommentId(CommentID);
            request.setIsUpdate(false);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> CreateComment([FromBody]CreateCommentRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult> GetComments([FromQuery] GetCommentsRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }
    }
}