using EmployeeService.Application.App.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Infrastructure.Data.Cache
{
    public sealed class ObjectCache : IObjectCache
    {
        private static readonly string Prefix = $"NID:{Environment.GetEnvironmentVariable("SERVICE_NAME")}";
        private readonly IDistributedCache _cache;
        public ObjectCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetCacheValueAsync<T>(string key, CancellationToken token = default) where T : class
        {
            try
            {
                string cacheKey = $"{Prefix}:{key}";
                string result = await _cache.GetStringAsync(cacheKey, token);
                if (string.IsNullOrEmpty(result))
                {
                    return null;
                }
                var deserializedObj = JsonSerializer.Deserialize<T>(result);
                return deserializedObj;
            }
            catch (Exception)
            {
                return null;
            }
        }
                
        public async Task SetCacheValueAsync<T>(string key, T value, CancellationToken token = default) where T : class
        {
            try
            {
                var cacheKey = $"{Prefix}:{key}";
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    // Remove item from cache after duration
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60),

                    // Remove item from cache if unsued for the duration
                    SlidingExpiration = TimeSpan.FromSeconds(30)
                };

                var result = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(cacheKey, result, cacheEntryOptions, token);
            }
            catch { }
        }

        public async Task RemoveValueAsync(string key, CancellationToken token = default)
        {
            try
            {
                var cacheKey = $"{Prefix}:{key}";
                await _cache.RemoveAsync(cacheKey, token);
            }
            catch { }
        }
    }
}
