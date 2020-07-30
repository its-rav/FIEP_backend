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
            var listOfEventSubs = _unitOfWork.Repository<GroupSubscription>().FindAllByProperty(x => x.UserId == request.UserId && x.IsDeleted == false);
            if (!listOfEventSubs.Any())
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            var listOfId = new List<dynamic>();
            foreach (var item in listOfEventSubs)
            {
                var sub = new
                {
                    GroupId = item.GroupId,
                    GroupName = item.Group.GroupName,
                    SubscriptionType = item.SubscriptionType
                };
                listOfId.Add(sub);
            }
            return new ResponseBase()
            {
                Response = listOfId
            };
        }
    }
}
