using BusinessTier.DistributedCache;
using BusinessTier.Fields;
using BusinessTier.Request;
using BusinessTier.Response;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class GetPostsOfEventHandler : IRequestHandler<GetPostsOfEventRequest, ResponseBase>
    {
        private readonly ICacheStore _cacheStore;
        private IUnitOfWork _unitOfWork;
        public GetPostsOfEventHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }
        public async Task<ResponseBase> Handle(GetPostsOfEventRequest request, CancellationToken cancellationToken)
        {
            IQueryable<Post> listPostsAfterSearch;
            if (request.GetEventId() == 0)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
                listPostsAfterSearch = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.EventId == request.GetEventId() && x.IsDeleted == false);
                if (listPostsAfterSearch.ToList().Count <= 0)
                {
                    return new ResponseBase()
                    {
                        Response = null
                    };
                }

            //apply sort
            var listPostsAfterSort = new List<Post>();
            switch (request.SortBy)
            {
                case PostFields.CreateDate: //sort by time occur
                    if (request.isDesc)
                    {
                        listPostsAfterSort = listPostsAfterSearch.OrderByDescending(x => x.CreateDate).ToList();
                    }
                    else
                    {
                        listPostsAfterSort = listPostsAfterSearch.OrderBy(x => x.CreateDate).ToList();
                    }
                    break;
            }

            //apply paging
            var listPostsAfterPaging = listPostsAfterSort
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();
            var listOfPosts = new List<dynamic>();
            foreach (var item in listPostsAfterPaging)
            {
                switch (request.FieldSize)
                {
                    case "short":
                        var postObj = new
                        {
                            postId = item.PostId,
                            postContent = item.PostContent
                        };

                        listOfPosts.Add(postObj);
                        break;
                    case "medium":
                        var postObjm = new
                        {
                            postId = item.PostId,
                            postContent = item.PostContent,
                            imageUrl = item.ImageUrl,
                            createDate = item.CreateDate
                        };
                        listOfPosts.Add(postObjm);
                        break;
                    default:
                        var postObjl = new
                        {
                            postId = item.PostId,
                            postContent = item.PostContent,
                            imageUrl = item.ImageUrl,
                            createDate = item.CreateDate,
                            eventId = item.EventId
                        };

                        listOfPosts.Add(postObjl);
                        break;
                }
            }
            var response = new
            {
                data = listOfPosts,
                currentPage = request.PageNumber,
                totalPages = Math.Ceiling((double)listPostsAfterSearch.ToList().Count / request.PageSize)
            };
            return new ResponseBase()
            {
                Response = response
            };
        }

        
    }
}
