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
    public class DeletePostHandler : IRequestHandler<DeletePostRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeletePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(DeletePostRequest request, CancellationToken cancellationToken)
        {
            var existingPost = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId == request.PostId && x.IsDeleted == false);
            if (existingPost == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            existingPost.IsDeleted = true;
            _unitOfWork.Repository<Post>().Update(existingPost);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
