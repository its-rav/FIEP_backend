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
    public class UsersController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public UsersController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers([FromQuery] GetUsersRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{UserId}")]
        public async Task<ActionResult> GetUser([FromRoute] GetUserByIdRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }
        [HttpPatch("{UserId}")]
        public async Task<ActionResult> UpdateUser([FromRoute]Guid UserId,[FromBody] UpdateUserRequest request)
        {
            request.setUserId(UserId);
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{UserId}")]
        public async Task<ActionResult> DeleteUser([FromRoute] DeleteUserRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
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