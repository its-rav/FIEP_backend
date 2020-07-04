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
using System.Web.Http.ModelBinding;

namespace BusinessTier.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            var newUser = new UserInformation()
            {
                RoleId =(int) request.RoleId,
                AvatarUrl = request.AvatarUrl,
                Fullname = request.FullName,
                Email = request.Email
            };
            CopyValues<UserInformation>(patchDoc, newUser);
            if (patchDoc != null)
            {
                var existingUser = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.UserId == request.getUserId() && x.IsDeleted == false);

                if(existingUser == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                patchDoc.ApplyTo(existingUser);
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
                if (value != null && !prop.Name.Contains("UserId"))
                {
                    if (prop.Name.Contains("RoleId") && (int)prop.GetValue(source, null) != 0)
                    {
                        patchDoc.Replace(prop.Name, value);
                    }
                    else if (!prop.Name.Contains("RoleId"))
                    {
                        patchDoc.Replace(prop.Name, value);
                    }
                }
            }
        }
    }
}
