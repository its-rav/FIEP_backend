using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetUserRecordAsync([FromQuery]String uid)
        {
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            // See the UserRecord reference doc for the contents of userRecord.
            return Ok(userRecord);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser()
        {
            UserRecordArgs args = new UserRecordArgs()
            {
                Uid = "asdasdsadasd222312asd123rasd",
                Email = "vippro0908@gmail.com",
                PhoneNumber = "+11234567890",
            };
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
            // See the UserRecord reference doc for the contents of userRecord.
            return Ok(args.Uid);
        }
    }
}