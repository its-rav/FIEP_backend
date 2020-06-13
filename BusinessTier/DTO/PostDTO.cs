using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class PostDTO
    {
        public Guid PostId { get; set; }
        public string PostContent { get; set; }
        public string ImageUrl { get; set; }

        public List<CommentDTO> listOfComments { get; set; }
    }
}
