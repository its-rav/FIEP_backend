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
    public class UpdatePostHandler : IRequestHandler<UpdatePostRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(UpdatePostRequest request, CancellationToken cancellationToken)
        {
            if (request.patchDoc != null)
            {
                var existingPost = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId == request.getPostId() && x.IsDeleted == false);

                if (existingPost == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                request.patchDoc.ApplyTo(existingPost);
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
