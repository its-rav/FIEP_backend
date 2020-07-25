using BusinessTier.DTO;
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
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPostByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(GetPostByIdRequest request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId.Equals(request.PostId) && x.IsDeleted == false);
            if (result == null)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            PostDTO postDTO = new PostDTO()
            {
                PostId = result.PostId,
                PostContent = result.PostContent,
                ImageUrl = result.ImageUrl
            };
            return new ResponseBase()
            {
                Response = postDTO
            };
        }
    }
}
