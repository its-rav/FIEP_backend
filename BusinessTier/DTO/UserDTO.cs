using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class UserDTO
    {
        public Guid Userid { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string AvatarUlr { get; set; }
    }
}
