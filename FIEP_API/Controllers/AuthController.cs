using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.Request;
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
    public class AuthController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("login")]
        public async Task<ActionResult> VerifyGoogleLogin([FromBody]AuthRequest request)
        {
            //var valuesOfRequest = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.ToString());
            //string idToken;
            //valuesOfRequest.TryGetValue("idToken", out idToken);
            string idToken = request.idToken;
            string fcmToken = request.fcmToken;

            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            FirebaseToken decodedToken = await auth.VerifyIdTokenAsync(idToken);
            UserRecord userRecord = await auth.GetUserAsync(decodedToken.Uid);

            string displayName = userRecord.DisplayName;
            string email = userRecord.Email;
            if (!email.Substring(email.IndexOf("@") + 1).Equals("fpt.edu.vn"))
            {
                return Unauthorized();
            }
            string userFullName = displayName.Substring(0, displayName.IndexOf("(") - 1);

            var existingStudent = _unitOfWork.Repository<UserInformation>().FindFirstByProperty(x => x.Email.Equals(email));
            if (existingStudent == null)
            {
                UserInformation newUser = new UserInformation()
                {
                    UserId = Guid.NewGuid(),
                    RoleId = 2,
                    Email = email,
                    Fullname = userFullName,
                    IsDeleted = false,
                    CreateDate = DateTime.Now
                };
                _unitOfWork.Repository<UserInformation>().Insert(newUser);
                _unitOfWork.Commit();
            }

            string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(decodedToken.Uid);
            return Ok(customToken);
        }
    }
}