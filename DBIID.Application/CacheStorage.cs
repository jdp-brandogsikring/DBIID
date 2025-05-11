using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application
{
    public class CacheStorage
    {
        private readonly MemoryCache _cache;

        private static CacheStorage _instance;
        private CacheStorage()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        public static CacheStorage Instance => _instance ??= new CacheStorage();

        public void Set<T>(string key, T value)
        {
            _cache.Set(key, value, TimeSpan.FromMinutes(5));
        }

        public T Get<T>(string key)
        {
            var item = _cache.Get<T>(key);
            if (item is not null)
            {
                return item;
            }
            return default;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

    }
}
