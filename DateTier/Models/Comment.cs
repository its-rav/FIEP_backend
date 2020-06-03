using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class Comment
    {
        public string CommentId { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public Guid CommentOwnerId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual User CommentOwner { get; set; }
        public virtual Post Post { get; set; }
    }
}
