using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class Role
    {
        public Role()
        {
            UserInformation = new HashSet<UserInformation>();
        }

        public int RoleId { get; set; }
        public string Rolename { get; set; }
        public virtual ICollection<UserInformation> UserInformation { get; set; }
    }
}
