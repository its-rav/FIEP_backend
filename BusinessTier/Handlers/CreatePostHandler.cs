using BusinessTier.Request;
using BusinessTier.Response;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class CreatePostHandler : IRequestHandler<CreatePostRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            var newPost = new Post()
            {
                EventId = request.EventId,
                PostContent = request.PostContent,
                ImageUrl = request.ImageUrl
            };
            Guid? newPostId = null;
            Boolean flag = false;
            while (flag == false)
            {
                newPostId = Guid.NewGuid();
                var existingPost = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId == newPostId);
                if (existingPost == null)
                {
                    flag = true;
                }
            }
            newPost.PostId = (Guid)newPostId;
            _unitOfWork.Repository<Post>().Insert(newPost);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
