namespace Fanex.Athena.Common.Utility.Services
{
    using System;
    using Interfaces;
    using Internal;
    using Newtonsoft.Json;

    public class MemoryCacheService : IMemoryCacheService
    {
        private const int TIMEOUT = 900; // 15 minutes.
        private const string REGIONNAME = "LAP_BETLIST_MAIN_MEMORY";
        private readonly ICustomMemoryCache _cache;

        public MemoryCacheService(ICustomMemoryCache cache)
        {
            _cache = cache;
        }

        public bool Contains(string key)
        {
            return _cache.Contains(key, REGIONNAME);
        }

        public T Get<T>(string key)
        {
            return Contains(key) ? DeepClone((T)_cache.Get(key, REGIONNAME)) : default(T);
        }

        public T Get<T>(string key, Func<T> action, int seconds = TIMEOUT) where T : class
        {
            var data = Get<T>(key);
            if (data == null)
            {
                data = action();
                Set(key, data, seconds);
            }

            return data;
        }

        public void Set<T>(string key, T value, int seconds = TIMEOUT)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key), "Key should not be empty!");
            }

            if (!Equals(value, default(T)))
            {
                _cache.Set(key, DeepClone(value), DateTime.Now.AddSeconds(seconds), REGIONNAME);
            }
        }

        public void Clear()
        {
            _cache.Clear(REGIONNAME);
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}