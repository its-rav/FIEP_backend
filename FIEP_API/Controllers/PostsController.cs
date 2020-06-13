using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using DataTier.Models;
using DataTier.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public PostsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult GetPosts(GetPostsRequest request)
        {
            var listPostsAfterSearch = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.IsDeleted == false);
            //apply paging
            var listPostsAfterPaging = listPostsAfterSearch
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();

            //apply sort
            var listPostsAfterSort = new List<Post>();
            switch (request.Field)
            {
                case PostFields.CreateDate: //sort by time occur
                    if (request.isDesc)
                    {
                        listPostsAfterSort = listPostsAfterPaging.OrderByDescending(x => x.CreateDate).ToList();
                    }
                    else
                    {
                        listPostsAfterSort = listPostsAfterPaging.OrderBy(x => x.CreateDate).ToList();
                    }
                    break;
            }

            var listOfPosts = new List<dynamic>();
            foreach (var item in listPostsAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "s":
                        var postObj = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent
                        };

                        listOfPosts.Add(postObj);
                        break;
                    case "m":
                        var postObjm = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent,
                            imageUrl = item.ImageUrl,
                            createDate = item.CreateDate
                        };
                        listOfPosts.Add(postObjm);
                        break;
                    default:
                        var postObjl = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent,
                            imageUrl = item.ImageUrl,
                            createDate = item.CreateDate,
                            eventId = item.EventId
                        };

                        listOfPosts.Add(postObjl);
                        break;
                }
            }
            return Ok(new
            {
                data = listOfPosts,
                totalPages = Math.Ceiling((double)listPostsAfterSearch.ToList().Count / request.PageSize)
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetPostById(Guid id)
        {
            var result = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId.Equals(id) && x.IsDeleted == false);
            PostDTO postDTO = new PostDTO()
            {
                PostId = result.PostId,
                PostContent = result.PostContent,
                ImageUrl = result.ImageUrl
            };
            return Ok(postDTO);
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
                    case "s":
                        var commentObj = new
                        {
                            commentId = item.CommentId,
                            commentContent = item.Content
                        };

                        listOfComments.Add(commentObj);
                        break;
                    case "m":
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