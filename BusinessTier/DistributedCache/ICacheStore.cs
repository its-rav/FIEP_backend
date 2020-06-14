using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.DistributedCache
{
    public interface ICacheStore
    {
        void Add(string key, object value);
        object Get(string Key);
        T Get<T>(string key);
        void Remove(string Key);
        bool IsExist(string Key);
    }
}
