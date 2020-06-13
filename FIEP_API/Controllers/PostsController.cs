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
        public ActionResult GetPostsOfEvent([FromQuery]GetPostsRequest request)
        {
            var listPostsAfterSearch = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.EventId == request.EventId && x.IsDeleted == false);
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

            var listOfPosts = new List<PostDTO>();
            foreach (var item in listPostsAfterSort)
            {
                PostDTO postDTO = new PostDTO()
                {
                    PostId = item.PostId,
                    PostContent = item.PostContent,
                    ImageUrl = item.ImageUrl
                };
                listOfPosts.Add(postDTO);
            }
            return Ok(new
            {
                data = listOfPosts,
                totalPages = (listPostsAfterSearch.ToList().Count / request.PageSize)
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetEvent(Guid id)
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
    }
}