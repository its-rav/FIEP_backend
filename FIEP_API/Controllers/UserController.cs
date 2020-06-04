using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.Response;
using DataTier.Models;
using DataTier.Repository;
using DataTier.UOW;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("login")]
        public async Task<ActionResult> GetUserRecordAsync([FromBody]Object request)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.ToString());
            string idToken;
            values.TryGetValue("idToken", out idToken);

            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            FirebaseToken decodedToken = await auth.VerifyIdTokenAsync(idToken);
            UserRecord userRecord = await auth.GetUserAsync(decodedToken.Uid);

            var existingStudent = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.Email.Equals(userRecord));
            //if(existingStudent == null)
            //{
            //    UserInformation newUser = new UserInformation()
            //    {
            //        UserId = Guid.NewGuid(),
            //        RoleId = 2,
            //        Email = 
            //    };
            //}

            string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(decodedToken.Uid);
            return Ok(new ResponseBase<UserRecord>()
            {
                ErrorCode = 0,
                Data = userRecord
            });
        }
    }
}