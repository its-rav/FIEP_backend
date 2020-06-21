﻿using BusinessTier.DistributedCache;
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
    public class GetPostsHandler : IRequestHandler<GetPostsRequest, ResponseBase>
    {
        private readonly ICacheStore _cacheStore;
        private IUnitOfWork _unitOfWork;
        public GetPostsHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }
        public async Task<ResponseBase> Handle(GetPostsRequest request, CancellationToken cancellationToken)
        {
            IQueryable<Post> listPostsAfterSearch;
            if (request.EventId != null)
            {
                listPostsAfterSearch = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.EventId == request.EventId && x.IsDeleted == false);
                if (listPostsAfterSearch.ToList().Count <= 0)
                {
                    return new ResponseBase()
                    {
                        Response = null
                    };
                }
            }
            else
            {
                listPostsAfterSearch = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.IsDeleted == false);
            }
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
                    case "short":
                        var postObj = new
                        {
                            postID = item.PostId,
                            postContent = item.PostContent
                        };

                        listOfPosts.Add(postObj);
                        break;
                    case "medium":
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
            var response = new
            {
                data = listOfPosts,
                totalPages = Math.Ceiling((double)listPostsAfterSearch.ToList().Count / request.PageSize)
            };
            return new ResponseBase()
            {
                Response = response
            };
        }
    }
}