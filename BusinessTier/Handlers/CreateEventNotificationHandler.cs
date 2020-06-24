using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.Services;
using DataTier.UOW;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class CreateEventNotificationHandler : IRequestHandler<CreateEventNotificationRequest, ResponseBase>
    {
        private IUnitOfWork _unitOfWork;
        private NotificationPublisher _notificationPublisher;
        public CreateEventNotificationHandler(IUnitOfWork unitOfWork, NotificationPublisher notificationPublisher)
        {
            _unitOfWork = unitOfWork;
            _notificationPublisher = notificationPublisher;
        }
        public async Task<ResponseBase> Handle(CreateEventNotificationRequest request, CancellationToken cancellationToken)
        {
            if (request.Body.Trim() == "" || request.Title.Trim() == "")
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }

            if (request.ImageUrl != null && request.ImageUrl.Trim() == "")
            {
                request.ImageUrl = null;
            }

            DataTier.Models.Notification notification = new DataTier.Models.Notification()
            {
                NotificationID = new Guid(),
                Body = request.Body,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                EventId=request.GetEventId()
            };
            
            _unitOfWork.Repository<DataTier.Models.Notification>().Insert(notification);
            _unitOfWork.Commit();

            _notificationPublisher.Publish(notification.NotificationID.ToString());
            return new ResponseBase()
            {
                Response = notification.NotificationID.ToString()
            };
        }
    }
}
