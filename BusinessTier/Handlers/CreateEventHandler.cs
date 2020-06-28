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
    public class CreateEventHandler : IRequestHandler<CreateEventRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var newEvent = new Event()
            {
                EventName = request.EventName,
                ImageUrl = request.EventImageUrl,
                Location = request.Location,
                TimeOccur = request.TimeOccur,
                GroupId = request.GroupId,
            };
            if(DateTime.Compare(DateTime.UtcNow, (DateTime)newEvent.TimeOccur) > 0)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            _unitOfWork.Repository<Event>().Insert(newEvent);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
