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
    public class CreateCommentHandler : IRequestHandler<CreateCommentRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
        {
            Boolean flag = false;
            Guid? commentId = null;
            while(flag == false)
            {
                commentId = Guid.NewGuid();
                var existingComment = _unitOfWork.Repository<Comment>().FindFirstByProperty(x => Guid.Parse(x.CommentId) == commentId);
                if(existingComment == null)
                {
                    flag = true;
                }
            }
            var newComment = new Comment()
            {
                CommentId = commentId.ToString(),
                Content = request.Content,
                PostId = request.PostID,
                CommentOwnerId = request.UserId
            };
            _unitOfWork.Repository<Comment>().Insert(newComment);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
