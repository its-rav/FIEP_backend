using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DTO
{
    public class CommentDTO
    {
        public string CommentId { get; set; }
        public string Content { get; set; }
        public Guid CommentOwnerId { get; set; }
    }
}
