using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessTier.DTO;
using BusinessTier.Request;
using DataTier.Models;
using DataTier.Repository;
using DataTier.UOW;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public AuthController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("login")]
        public async Task<ActionResult> VerifyGoogleLogin([FromBody]AuthRequest request)
        {
            string idToken = request.idToken;
            string fcmToken = "";
            if (request.fcmToken != null)
            {
                fcmToken = request.fcmToken;
            }
            
            Dictionary<string, object> customeUserClaims = new Dictionary<string, object>();

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
            var existingFCMToken = _unitOfWork.Repository<UserFCMToken>().FindFirstByProperty(x => x.FCMToken.Equals(fcmToken));

            string indentity = "";
            int roleId;
            UserDTO userToResponse;
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
                //get info to response
                userToResponse = new UserDTO()
                {
                    UserId = newUser.UserId,
                    //AvatarUrl = newUser.AvatarUrl,
                    FullName = newUser.Fullname,
                    Mail = newUser.Email
                };
                _unitOfWork.Repository<UserInformation>().Insert(newUser);
                if (!fcmToken.Equals(""))
                {
                    if (existingFCMToken == null)
                    {
                        UserFCMToken userFCMToken = new UserFCMToken()
                        {
                            FCMToken = fcmToken,
                            UserID = newUser.UserId,
                        };
                        _unitOfWork.Repository<UserFCMToken>().Insert(userFCMToken);
                    }
                    else
                    {
                        existingFCMToken.UserID = newUser.UserId;
                        _unitOfWork.Repository<UserFCMToken>().Update(existingFCMToken);
                    }
                }
                indentity = newUser.UserId.ToString();
                roleId = 2;
            }
            else
            {
                userToResponse = new UserDTO()
                {
                    UserId = existingStudent.UserId,
                    //AvatarUrl = existingStudent.AvatarUrl,
                    FullName = existingStudent.Fullname,
                    Mail = existingStudent.Email
                };
                if (!fcmToken.Equals(""))
                {
                    if (existingFCMToken == null)
                    {
                        UserFCMToken userFCMToken = new UserFCMToken()
                        {
                            FCMToken = fcmToken,
                            UserID = existingStudent.UserId,
                        };
                        _unitOfWork.Repository<UserFCMToken>().Insert(userFCMToken);
                    }
                    else
                    {
                        existingFCMToken.UserID = existingStudent.UserId;
                        _unitOfWork.Repository<UserFCMToken>().Update(existingFCMToken);
                    }
                }
                indentity = existingStudent.UserId.ToString();
                roleId = existingStudent.RoleId;
            }
            _unitOfWork.Commit();

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            IdentityModelEventSource.ShowPII = true;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Identity",indentity),
                    new Claim("RoleId",roleId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512),
                Issuer = _config["Tokens:Issuer"],
                Audience = _config["Tokens:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);
            return Ok(new { 
                UserInfo = userToResponse,
                Token = result
            });
        }
    }
}