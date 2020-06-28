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
    public class DeleteEventHandler : IRequestHandler<DeleteEventRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(DeleteEventRequest request, CancellationToken cancellationToken)
        {
            var existingEvent = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == request.EventId && x.IsDeleted == false);
            if (existingEvent == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            existingEvent.IsDeleted = true;
            _unitOfWork.Repository<Event>().Update(existingEvent);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
