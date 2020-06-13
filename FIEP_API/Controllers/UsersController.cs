using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using DataTier.Models;
using DataTier.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult GetUsers([FromQuery] GetUsersRequest request)
        {        
            var listUsersAfterFilter = _unitOfWork.Repository<UserInformation>().GetAll().Where(x => x.IsDeleted == false);
            if (request.SearchParam.Length > 0)
            {
                listUsersAfterFilter = listUsersAfterFilter.Where(x => x.Fullname.Contains(request.SearchParam));
            }

            //apply paging
            var listUsersAfterPaging = listUsersAfterFilter
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            //apply sort
            var listUsersAfterSort = new List<UserInformation>();
            switch (request.Field)
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

            var listOfUsers = new List<UserDTO>();
            foreach (var item in listUsersAfterSort)
            {
                UserDTO userDTO = new UserDTO()
                {
                    Name = item.Fullname,
                    Mail = item.Email,
                    Userid = item.UserId,
                    AvatarUlr = item.AvatarUrl
                };
                listOfUsers.Add(userDTO);
            }
            return Ok(new
            {
                data = listOfUsers,
                totalPages = (listUsersAfterFilter.ToList().Count / request.PageSize)
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetEvent(Guid id)
        {
            var result = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.UserId.Equals(id) && x.IsDeleted == false);
            UserDTO userDTO = new UserDTO()
            {
                Name = result.Fullname,
                Mail = result.Email,
                Userid = result.UserId,
                AvatarUlr = result.AvatarUrl
            };
            return Ok(userDTO);
        }
    }
}