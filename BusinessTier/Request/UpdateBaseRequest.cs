using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class UpdateBaseRequest<T> where T : class
    {
        public JsonPatchDocument<T> patchDoc { get; set; }
    }
}
