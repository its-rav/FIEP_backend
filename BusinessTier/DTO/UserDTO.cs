using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Mail { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
    }
}
