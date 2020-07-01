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
    public class GetCommentsHandler : IRequestHandler<GetCommentsRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCommentsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(GetCommentsRequest request, CancellationToken cancellationToken)
        {
            var listCommentsAfterSearch = _unitOfWork.Repository<Comment>().FindAllByProperty(x => x.IsDeleted == false);
            if (listCommentsAfterSearch.Count() <= 0)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            //apply paging
            var listCommentsAfterPaging = listCommentsAfterSearch
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();

            //apply sort
            var listCommentsAfterSort = new List<Comment>();
            switch (request.SortBy)
            {
                case CommentFields.CreateDate:
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
            var result = new
            {
                data = listOfComments,
                currentPage = request.PageNumber,
                totalPages = Math.Ceiling((double)listCommentsAfterSearch.ToList().Count / request.PageSize)
            };
            return new ResponseBase()
            {
                Response = result
            };
        }
    }
}
