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
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCommentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
        {
            var existingComment = _unitOfWork.Repository<Comment>().FindFirstByProperty(x => Guid.Parse(x.CommentId) == request.CommentId && x.IsDeleted == false);
            existingComment.IsDeleted = true;
            _unitOfWork.Repository<Comment>().Update(existingComment);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
