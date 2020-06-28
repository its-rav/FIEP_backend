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
    public class UpdateAndCreatePostHandler : IRequestHandler<UpdateOrCreatePostRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAndCreatePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(UpdateOrCreatePostRequest request, CancellationToken cancellationToken)
        {
            var newPost = new Post()
            {
                EventId = request.EventId,
                PostContent = request.PostContent,
                ImageUrl = request.ImageUrl
            };
            if (request.getIsUpdate() == true)
            {
                var existingPost = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId == request.getPostId() && x.IsDeleted == false);
                if (existingPost == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                CopyValues<Post>(existingPost, newPost);
                existingPost.ModifyDate = DateTime.Now;
                _unitOfWork.Repository<Post>().Update(existingPost);
            }
            else
            {
                Guid? newPostId = null;
                Boolean flag = false;
                while(flag == false)
                {
                    newPostId = Guid.NewGuid();
                    var existingPost = _unitOfWork.Repository<Post>().FindFirstByProperty(x => x.PostId == request.getPostId() && x.IsDeleted == false);
                    if(existingPost == null)
                    {
                        flag = true;
                    }
                }
                newPost.PostId = (Guid)newPostId;
                _unitOfWork.Repository<Post>().Insert(newPost);
            }
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
        public void CopyValues<T>(T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null && !prop.Name.Contains("PostId"))
                {
                    if (prop.Name.Contains("EventId") && (int)prop.GetValue(source, null) != 0)
                    {
                        prop.SetValue(target, value, null);
                    }
                    else if (!prop.Name.Contains("EventId"))
                    {
                        prop.SetValue(target, value, null);
                    }
                }
            }
        }
    }
}
