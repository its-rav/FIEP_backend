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
    public class UpdateAndCreateGroupHandler : IRequestHandler<UpdateOrCreateGroupRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAndCreateGroupHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(UpdateOrCreateGroupRequest request, CancellationToken cancellationToken)
        {
            var newGroup = new GroupInformation()
            {
                GroupName = request.GroupName,
                GroupImageUrl = request.ImageUrl
            };
            if (request.getIsUpdate() == true)
            {
                var existingGroup = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == request.getGroupId() && x.IsDeleted == false);
                if (existingGroup == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                CopyValues<GroupInformation>(existingGroup, newGroup);
                existingGroup.ModifyDate = DateTime.Now;
                _unitOfWork.Repository<GroupInformation>().Update(existingGroup);
            }
            else
            {
                _unitOfWork.Repository<GroupInformation>().Insert(newGroup);
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
                if (value != null && !prop.Name.Contains("GroupId"))
                {
                    prop.SetValue(target, value, null);
                }
            }
        }
    }
}
