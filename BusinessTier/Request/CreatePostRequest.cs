using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class CreatePostRequest : IRequest<ResponseBase>
    {
        public int EventId { get; set; }
        public string PostContent { get; set; }
        public string ImageUrl { get; set; }
    }
}
