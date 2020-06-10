using System;
using System.Collections.Generic;

namespace DataTier.Models
{
    public partial class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
        }

        public Guid PostId { get; set; }
        public int EventId { get; set; }
        public string PostContent { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public Boolean IsDeleted { get; set; }
        public virtual Event Event { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
    }
}
