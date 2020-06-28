using BusinessTier.DTO;
using BusinessTier.Fields;
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
    public class GetNotificationsHandler : IRequestHandler<GetNotificationsRequest, ResponseBase>
    {
        private IUnitOfWork _unitOfWork;
        public GetNotificationsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async  Task<ResponseBase> Handle(GetNotificationsRequest request, CancellationToken cancellationToken)
        {
            var listNotificationsAfterFilter = _unitOfWork.Repository<Notification>().GetAll();
            if (request.Query.Length > 0)
            {
                listNotificationsAfterFilter = listNotificationsAfterFilter.Where(x => x.Title.Contains(request.Query));
            }
            //apply paging
            var listNotificationsAfterPaging = listNotificationsAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var listNotificationssAfterSort = new List<Notification>();
            switch (request.SortBy)
            {
                case NotificationFields.CreateDate: //sort by number of follower
                    if (request.isDesc)
                    {
                        listNotificationssAfterSort = listNotificationsAfterPaging.OrderByDescending(x => x.CreateDate).ToList();
                    }
                    else
                    {
                        listNotificationssAfterSort = listNotificationsAfterPaging.OrderBy(x => x.CreateDate).ToList();
                    }
                    break;
            }

            var result = new List<NotificationDTO>();
            foreach (var item in listNotificationssAfterSort)
            {
                NotificationDTO notificationDTO = new NotificationDTO()
                {
                    NotificationID = item.NotificationID,
                    Title = item.Title,
                    Body = item.Body,
                    ImageUrl = item.ImageUrl,
                    EventId = item.EventId,
                    GroupId = item.GroupId,
                    UserFCMTokens = item.UserFCMTokens
                };
                result.Add(notificationDTO);
            }

            

            var response  = new
            {
                data = result,
                currentPage = request.PageNumber,
                totalPages = Math.Ceiling((double)listNotificationssAfterSort.ToList().Count / request.PageSize)
            };

            return new ResponseBase()
            {
                Response = response
            };
        }
    }
}
