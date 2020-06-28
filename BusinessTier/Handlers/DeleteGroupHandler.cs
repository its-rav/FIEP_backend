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
    public class DeleteGroupHandler : IRequestHandler<DeleteGroupRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGroupHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
        {
            var existingGroup = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == request.GroupId && x.IsDeleted == false);
            if (existingGroup == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            existingGroup.IsDeleted = true;
            _unitOfWork.Repository<GroupInformation>().Update(existingGroup);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
