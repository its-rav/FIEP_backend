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
    public class CreateNotificationHandler : IRequestHandler<CreateNotificationRequest, ResponseBase>
    {
        private IUnitOfWork _unitOfWork;
        private NotificationPublisher _notificationPublisher;
        public CreateNotificationHandler(IUnitOfWork unitOfWork, NotificationPublisher notificationPublisher)
        {
            _unitOfWork = unitOfWork;
            _notificationPublisher = notificationPublisher;
        }
        public async  Task<ResponseBase> Handle(CreateNotificationRequest request, CancellationToken cancellationToken)
        {
            DataTier.Models.Notification notification = new DataTier.Models.Notification()
            {
                NotificationID = new Guid(),
                Body = request.Body,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
            };
            if(request.GroupId != null)
            {
                notification.GroupId = request.GroupId;
            }
            else
            {
                notification.EventId = request.EventId;
            }
            _unitOfWork.Repository<DataTier.Models.Notification>().Insert(notification);
            _unitOfWork.Commit();

            _notificationPublisher.Publish(notification.NotificationID.ToString());
            return new ResponseBase()
            {
                Response = null
            };
        }
    }
}
