using BusinessTier.DistributedCache;
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
    public class GetGroupsHandler : IRequestHandler<GetGroupsRequest, ResponseBase>
    {
        private readonly ICacheStore _cacheStore;
        private IUnitOfWork _unitOfWork;
        public GetGroupsHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }
        public async Task<ResponseBase> Handle(GetGroupsRequest request, CancellationToken cancellationToken)
        {
            var listGroupAfterFilter = _unitOfWork.Repository<GroupInformation>().GetAll().Where(x => x.IsDeleted == false);
            if (listGroupAfterFilter.ToList().Count <= 0)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            if (request.Query.Length > 0)
            {
                listGroupAfterFilter = listGroupAfterFilter.Where(x => x.GroupName.Contains(request.Query));
            }
            //apply paging
            var listGroupsAfterPaging = listGroupAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            //apply sort
            var listGroupsAfterSort = new List<GroupInformation>();
            switch (request.Field)
            {
                case GroupFields.Follower: //sort by number of follower
                    if (request.isDesc)
                    {
                        listGroupsAfterSort = listGroupsAfterPaging.OrderByDescending(x => x.GroupSubscription.Count).ToList();
                    }
                    else
                    {
                        listGroupsAfterSort = listGroupsAfterPaging.OrderBy(x => x.GroupSubscription.Count).ToList();
                    }
                    break;
            }

            var listOfGroups = new List<dynamic>();
            foreach (var item in listGroupsAfterSort)
            {
                switch (request.FieldSize)
                {
                    case "short":
                        var groupObj = new
                        {
                            groupID = item.GroupId,
                            groupName = item.GroupName
                        };

                        listOfGroups.Add(groupObj);
                        break;
                    case "medium":
                        var groupObjm = new
                        {
                            groupID = item.GroupId,
                            groupName = item.GroupName,
                            imageUrl = item.GroupImageUrl,
                        };
                        listOfGroups.Add(groupObjm);
                        break;
                    default:
                        var groupObjl = new
                        {
                            groupID = item.GroupId,
                            groupName = item.GroupName,
                            groupImageUrl = item.GroupImageUrl,
                            groupFollower = item.GroupSubscription.Count,
                            manager = item.GroupManagerId,
                        };
                        listOfGroups.Add(groupObjl);
                        break;
                }
            }
            var response = new
            {
                data = listOfGroups,
                totalPages = Math.Ceiling((double)listGroupAfterFilter.ToList().Count / request.PageSize)
            };
            return new ResponseBase()
            {
                Response = response
            };
        }
    }
}
