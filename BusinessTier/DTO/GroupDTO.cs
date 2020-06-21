using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupImageUrl { get; set; }
        public int GroupFollower { get; set; }
        public List<EventDTO> listOfEvents { get; set; }
    }
}
