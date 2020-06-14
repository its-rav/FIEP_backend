using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DistributedCache
{
    public class NoApplyCache :  ICacheStore  
    {
        public void Add(string key, object value)
        {
        }

        public object Get(string key)
        {
            return null;
        }

        public bool IsExist(string Key)
        {
            return false;
        }

        public void Remove(string key)
        {

        }

        public T Get<T>(string key)
        {
            return default(T);
        }
    }
}
