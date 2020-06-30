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
    public class UpdateAndDeleteCommentHandler : IRequestHandler<UpdateOrDeleteCommentRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAndDeleteCommentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(UpdateOrDeleteCommentRequest request, CancellationToken cancellationToken)
        {
            var existingComment = _unitOfWork.Repository<Comment>().FindFirstByProperty(x => Guid.Parse(x.CommentId) == request.getCommentId() && x.IsDeleted == false);
            if (existingComment == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            if (request.getIsUpdate() == true)
            {
                existingComment.Content = request.Content;              
            }
            else
            {
                existingComment.IsDeleted = true;
            }
            _unitOfWork.Repository<Comment>().Update(existingComment);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
