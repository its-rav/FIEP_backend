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
    public class GetNotificationByIdHandler : IRequestHandler<GetNotificationByIdRequest, ResponseBase>
    {
        private IUnitOfWork _unitOfWork;
        public GetNotificationByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(GetNotificationByIdRequest request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Repository<Notification>().FindFirstByProperty(x => x.NotificationID.Equals(request.NotiID));
            if(result == null)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            NotificationDTO notificationDTO = new NotificationDTO()
            {
                NotificationID = result.NotificationID,
                Title = result.Title,
                Body = result.Body,
                ImageUrl = result.ImageUrl,
                EventId = result.EventId,
                GroupId = result.GroupId,
                UserFCMTokens = result.UserFCMTokens
            };
            return new ResponseBase()
            {
                Response = notificationDTO
            };
        }
    }
}
