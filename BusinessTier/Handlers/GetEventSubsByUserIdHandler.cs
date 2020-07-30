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
    public class GetEventSubsByUserIdHandler : IRequestHandler<GetEventSubsByUserIdRequest,ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEventSubsByUserIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> Handle(GetEventSubsByUserIdRequest request, CancellationToken cancellationToken)
        {
            var listOfEventSubs = _unitOfWork.Repository<EventSubscription>().FindAllByProperty(x => x.UserId == request.UserId && x.IsDeleted == false);
            if(!listOfEventSubs.Any())
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
                    EventId = item.EventId,
                    EventName = item.Event.EventName,
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
