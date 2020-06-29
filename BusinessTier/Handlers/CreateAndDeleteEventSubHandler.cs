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
    public class CreateAndDeleteEventSubHandler : IRequestHandler<CreateOrDeleteEventSubRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateAndDeleteEventSubHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(CreateOrDeleteEventSubRequest request, CancellationToken cancellationToken)
        {
            var existingEventSub = _unitOfWork.Repository<EventSubscription>().FindFirstByProperty(x => x.EventId == request.EventId
                                                                                                      &&  x.UserId == request.UserId);
            if (request.getIsCreate() == true)
            {
                if (existingEventSub != null && existingEventSub.IsDeleted == true)
                {
                    existingEventSub.IsDeleted = false;
                    _unitOfWork.Repository<EventSubscription>().Update(existingEventSub);
                }
                else if (existingEventSub == null)
                {
                    var newEventSub = new EventSubscription()
                    {
                        EventId = request.EventId,
                        UserId = request.UserId
                    };
                    _unitOfWork.Repository<EventSubscription>().Insert(newEventSub);
                }
            }
            else
            {
                if (existingEventSub != null && existingEventSub.IsDeleted == false)
                {
                    existingEventSub.IsDeleted = true;
                    _unitOfWork.Repository<EventSubscription>().Update(existingEventSub);
                }
                else if(existingEventSub == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
            }

            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
