using BusinessTier.Request;
using BusinessTier.Response;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
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
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            var newPost = new Post()
            {
                PostContent = request.PostContent,
                ImageUrl = request.ImageUrl
            };
            CopyValues<Post>(patchDoc, newPost);
            if (patchDoc != null)
            {
                var existingPost = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId == request.getPostId() && x.IsDeleted == false);

                if (existingPost == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                patchDoc.ApplyTo(existingPost);
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
        public void CopyValues<T>(JsonPatchDocument patchDoc, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null && !prop.Name.Contains("Id"))
                {
                    patchDoc.Replace(prop.Name, value);
                }
            }
        }
    }
}
