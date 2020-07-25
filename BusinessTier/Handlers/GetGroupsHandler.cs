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
        const string groupCacheKey = "GroupsTable";
        public GetGroupsHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }
        public async Task<ResponseBase> Handle(GetGroupsRequest request, CancellationToken cancellationToken)
        {
            List<GroupInformation> listGroupAfterFilter;
            if(!_cacheStore.IsExist(groupCacheKey))
            {
                listGroupAfterFilter = _unitOfWork.Repository<GroupInformation>().GetAll().Where(x => x.IsDeleted == false).ToList();
            }
            else
            {
                listGroupAfterFilter = _cacheStore.Get<List<GroupInformation>>(groupCacheKey).Where(x => x.IsDeleted == false).ToList();
            }
            if (listGroupAfterFilter.Count <= 0)
            {
                return new ResponseBase()
                {
                    Response = null
                };
            }
            if(request.Query != null)
            {
                if (request.Query.Trim().Length > 0)
                {
                    listGroupAfterFilter = listGroupAfterFilter.Where(x => x.GroupName.Contains(request.Query.Trim())).ToList();
                    if (listGroupAfterFilter.Count() <= 0)
                    {
                        return new ResponseBase()
                        {
                            Response = null
                        };
                    }
                }
            }
            
            //apply paging
            var listGroupsAfterPaging = listGroupAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            //apply sort
            var listGroupsAfterSort = new List<GroupInformation>();
            switch (request.SortBy)
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
                        };
                        listOfGroups.Add(groupObjl);
                        break;
                }
            }
            var response = new
            {
                data = listOfGroups,
                currentPage = request.PageNumber,
                totalPages = Math.Ceiling((double)listGroupAfterFilter.ToList().Count / request.PageSize)
            };
            return new ResponseBase()
            {
                Response = response
            };
        }
    }
}
