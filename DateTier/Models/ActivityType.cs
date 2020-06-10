using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class ActivityType
    {
        public int ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
