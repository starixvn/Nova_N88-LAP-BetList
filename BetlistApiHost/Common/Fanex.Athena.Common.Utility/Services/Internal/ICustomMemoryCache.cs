using System;
using System.Runtime.Caching;

namespace Fanex.Athena.Common.Utility.Services.Internal
{
    public interface ICustomMemoryCache
    {
        DefaultCacheCapabilities DefaultCacheCapabilities { get; }

        void Clear(string region);

        bool Contains(string key, string regionName = null);

        object Get(string key, string regionName = null);

        CacheItem GetCacheItem(string key, string regionName = null);

        void Set(CacheItem item, CacheItemPolicy policy);

        void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

        void Set(string key, object value, CacheItemPolicy policy, string regionName = null);
    }
}