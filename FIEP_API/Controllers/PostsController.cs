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
        public ActionResult GetCommentsOfPost(Guid PostId,[FromQuery]GetCommentsRequest request)
        {
            var listCommentsAfterSearch = _unitOfWork.Repository<Comment>().FindAllByProperty(x => x.PostId.Equals(PostId) && x.IsDeleted == false);
            //apply paging
            var listCommentsAfterPaging = listCommentsAfterSearch
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();

            //apply sort
            var listCommentsAfterSort = new List<Comment>();
            switch (request.Field)
            {
                case CommentFields.CreateDate: //sort by time occur
                    if (request.isDesc)
                    {
                        listCommentsAfterSort = listCommentsAfterPaging.OrderByDescending(x => x.CreateDate).ToList();
                    }
                    else
                    {
                        listCommentsAfterSort = listCommentsAfterPaging.OrderBy(x => x.CreateDate).ToList();
                    }
                    break;
            }

            var listOfComments = new List<dynamic>();
            foreach (var item in listCommentsAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "short":
                        var commentObj = new
                        {
                            commentId = item.CommentId,
                            commentContent = item.Content
                        };

                        listOfComments.Add(commentObj);
                        break;
                    case "medium":
                        var commentObjm = new
                        {
                            commentId = item.CommentId,
                            commentContent = item.Content,
                            postId = item.PostId,
                            userId = item.CommentOwnerId
                        };
                        listOfComments.Add(commentObjm);
                        break;
                    default:
                        var commentObjl = new
                        {
                            commentId = item.CommentId,
                            commentContent = item.Content,
                            postId = item.PostId,
                            userId = item.CommentOwnerId,
                            createDate = item.CreateDate
                        };

                        listOfComments.Add(commentObjl);
                        break;
                }
            }
            return Ok(new
            {
                data = listOfComments,
                totalPages = Math.Ceiling((double)listCommentsAfterSearch.ToList().Count / request.PageSize)
            });
        }
    }
}