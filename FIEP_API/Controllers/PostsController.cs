using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public PostsController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetPosts([FromQuery]GetPostsRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{PostId}")]
        public async  Task<ActionResult> GetPostById([FromRoute]GetPostByIdRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }


        [HttpGet("{PostId}/comments")]
        public async Task<ActionResult> GetCommentsOfPost(Guid PostId,[FromQuery]GetCommentsRequest request)
        {
            request.setPostID(PostId);
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);           
        }
        [HttpPut("{postId}")]
        public async Task<ActionResult> UpdatePost([FromRoute]Guid postId, [FromBody]UpdateOrCreatePostRequest request)
        {
            request.setPostId(postId);
            request.setIsUpdate(true);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody]UpdateOrCreatePostRequest request)
        {
            request.setIsUpdate(false);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{PostId}")]
        public async Task<ActionResult> DeletePost([FromRoute]DeletePostRequest request)
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