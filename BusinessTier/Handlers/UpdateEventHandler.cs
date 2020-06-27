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
            var existingEvent = _unitOfWork.Repository<Event>().FindFirstByProperty(x => x.EventId == request.getEventId());
            if(existingEvent == null)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            var newEvent = new Event()
            {
                EventName = request.EventName,
                ImageUrl = request.EventImageUrl,
                ApprovalState = request.ApprovalState,    
                Location = request.Location,
                TimeOccur = request.TimeOccur,
                GroupId = request.GroupId,
                IsExpired = request.IsExpired
            };
            CopyValues<Event>(existingEvent, newEvent);
            existingEvent.ModifyDate = DateTime.Now;
            _unitOfWork.Repository<Event>().Update(existingEvent);
            _unitOfWork.Commit();
            return new ResponseBase()
            {
                Response = 1
            };
        }
        public void CopyValues<T>(T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null && !prop.Name.Contains("EventId"))
                {
                    if(prop.Name.Contains("GroupId") && (int)prop.GetValue(source, null) != 0)
                    {
                        prop.SetValue(target, value, null);
                    }
                    else if(!prop.Name.Contains("GroupId"))
                    {
                        prop.SetValue(target, value, null);
                    }                 
                }                   
            }
        }
    }
}
