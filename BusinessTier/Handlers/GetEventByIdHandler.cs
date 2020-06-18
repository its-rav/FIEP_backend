using BusinessTier.DTO;
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
    public class GetEventByIdHandler : IRequestHandler<GetEventByIdRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEventByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(GetEventByIdRequest request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == request.EventId && x.IsDeleted == false);
            if(result == null)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            EventDTO eventDTO = new EventDTO()
            {
                EventId = result.EventId,
                EventName = result.EventName,
                EventImageUrl = result.ImageUrl,
                TimeOccur = (DateTime)result.TimeOccur
            };
            return new ResponseBase()
            {
                Response = eventDTO
            };
        }
    }
}
