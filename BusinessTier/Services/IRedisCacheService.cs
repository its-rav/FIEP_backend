using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Services
{
    public interface IRedisCacheService
    {
        public void CacheGroupTable();
        public void CacheEventTable();
    }
}
