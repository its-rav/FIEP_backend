using BusinessTier.Request;
using BusinessTier.Response;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCommentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Replace("Content", request.Content);
            if (patchDoc != null)
            {
                var existingComment = _unitOfWork.Repository<Comment>().FindFirstByProperty(x => Guid.Parse(x.CommentId) == request.getCommentId() && x.IsDeleted == false);

                if (existingComment == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                patchDoc.ApplyTo(existingComment);
                _unitOfWork.Commit();
                return new ResponseBase()
                {
                    Response = 1
                };
            }
            else
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
        }
    }
}
