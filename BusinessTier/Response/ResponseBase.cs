using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Response
{
    public class ResponseBase<T>
    {
        public ResponseBase(T data)
        {
            Data = data;
        }

        public ResponseBase() { }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

}
