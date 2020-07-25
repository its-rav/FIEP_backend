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
    public class GetGroupSubsByUserIdHandler : IRequestHandler<GetGroupSubsByUserIdRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetGroupSubsByUserIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(GetGroupSubsByUserIdRequest request, CancellationToken cancellationToken)
        {
            var listOfEventSubs = _unitOfWork.Repository<GroupSubscription>().FindAllByProperty(x => x.UserId == request.UserId);
            if (!listOfEventSubs.Any())
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            var listOfId = listOfEventSubs.Select(x => x.GroupId).ToList();
            return new ResponseBase()
            {
                Response = listOfId
            };
        }
    }
}
