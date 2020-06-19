using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class GroupStatisticDTO
    {

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public int Followers { get; set; }

        public int eventsCount { get; set; }
        public int activeEventsCount { get; set; }
    }
}
