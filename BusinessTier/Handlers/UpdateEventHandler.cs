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
    public class UpdateEventHandler : IRequestHandler<UpdateEventRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            if(request.patchDoc != null)
            {
                var existingEvent = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == request.getEventId() && x.IsDeleted == false);

                if (existingEvent == null)
                {
                    return new ResponseBase()
                    {
                        Response = 0
                    };
                }
                request.patchDoc.ApplyTo(existingEvent);
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
