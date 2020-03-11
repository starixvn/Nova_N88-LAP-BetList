namespace Fanex.Athena.Common.Utility.Services.Internal
{
    using System;
    using System.Linq;
    using System.Runtime.Caching;

    public class CustomMemoryCache : MemoryCache, ICustomMemoryCache
    {
        public CustomMemoryCache() : base("LAP_BETLIST_MEMORY_CUSTOM_CACHING")
        {
        }

        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return base.DefaultCacheCapabilities | DefaultCacheCapabilities.CacheRegions;
            }
        }

        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            Set(item.Key, item.Value, policy, item.RegionName);
        }

        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            Set(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration }, regionName);
        }

        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            base.Set(CreateKeyWithRegion(key, regionName), value, policy);
        }

        public override CacheItem GetCacheItem(string key, string regionName = null)
        {
            CacheItem temporary = base.GetCacheItem(CreateKeyWithRegion(key, regionName));
            return new CacheItem(key, temporary.Value, regionName);
        }

        public override object Get(string key, string regionName = null)
        {
            return base.Get(CreateKeyWithRegion(key, regionName));
        }

        public override bool Contains(string key, string regionName = null)
        {
            return base.Contains(CreateKeyWithRegion(key, regionName));
        }

        public void Clear(string region)
        {
            var keys = this.Where(i => i.Key.StartsWith($"region:{region}"))
                .Select(i => i.Key).ToList();
            foreach (var key in keys)
            {
                this.Remove(key);
            }
        }

        private static string CreateKeyWithRegion(string key, string regionName)
        {
            return $"region:{regionName ?? "null_region"};key={key}";
        }
    }
}
