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
    public class UpdateGroupHandler : IRequestHandler<UpdateGroupRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateGroupHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            if(request.patchDoc != null)
            {
                var existingGroup = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == request.getGroupId() && x.IsDeleted == false);

                if (existingGroup == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                request.patchDoc.ApplyTo(existingGroup);
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
