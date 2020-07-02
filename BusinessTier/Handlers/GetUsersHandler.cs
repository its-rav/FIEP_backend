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
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, ResponseBase>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUsersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var listUsersAfterFilter = _unitOfWork.Repository<UserInformation>().GetAll().Where(x => x.IsDeleted == false);
            if (request.Query != null)
            {
                if (request.Query.Trim().Length > 0)
                {
                    listUsersAfterFilter = listUsersAfterFilter.Where(x => x.Fullname.Contains(request.Query.Trim()));
                    if (listUsersAfterFilter.Count() <= 0)
                    {
                        return new ResponseBase()
                        {
                            Response = null
                        };
                    }
                }
            }           
            if(listUsersAfterFilter.Count() <= 0)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }

            //apply paging
            var listUsersAfterPaging = listUsersAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            //apply sort
            var listUsersAfterSort = new List<UserInformation>();
            switch (request.SortBy)
            {
                case UserInformationFields.FullName: //sort by time occur
                    if (request.isDesc)
                    {
                        listUsersAfterSort = listUsersAfterPaging.OrderByDescending(x => x.Fullname).ToList();
                    }
                    else
                    {
                        listUsersAfterSort = listUsersAfterPaging.OrderBy(x => x.Fullname).ToList();
                    }
                    break;
            }

            var listOfUsers = new List<dynamic>();
            foreach (var item in listUsersAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "short":
                        var userObj = new
                        {
                            userId = item.UserId,
                            fullName = item.Fullname
                        };

                        listOfUsers.Add(userObj);
                        break;
                    case "medium":
                        var userObjm = new
                        {
                            userId = item.UserId,
                            fullName = item.Fullname,
                            mail = item.Email
                        };
                        listOfUsers.Add(userObjm);
                        break;
                    default:
                        var userObjl = new
                        {
                            userId = item.UserId,
                            fullName = item.Fullname,
                            mail = item.Email,
                            avatarUrl = item.AvatarUrl
                        };
                        listOfUsers.Add(userObjl);
                        break;
                }
            }
            var response = new
            {
                data = listOfUsers,
                totalPages = Math.Ceiling((double)listUsersAfterFilter.ToList().Count / request.PageSize)
            };
            return new ResponseBase()
            {
                Response = response
            };
        }
    }
}
