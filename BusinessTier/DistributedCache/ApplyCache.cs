
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BusinessTier.DistributedCache
{
    public class ApplyCache : ICacheStore
    {
        private readonly IDistributedCache _distributedCache;

        public ApplyCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public void Add(string key, object value)
        {
           _distributedCache.SetString(key, JsonConvert.SerializeObject(value));
        }

        public object Get(string key)
        {
            var result = _distributedCache.GetString(key);
            return JsonConvert.DeserializeObject(result);
        }

        public T Get<T>(string key)
        {
            var result = _distributedCache.GetString(key);
            if(result == null)
            {
                return default(T);
            }
            return (T)JsonConvert.DeserializeObject(result, typeof(T));
        }

        public bool IsExist(string Key)
        {
            return _distributedCache.GetString(Key) != null;          
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}
