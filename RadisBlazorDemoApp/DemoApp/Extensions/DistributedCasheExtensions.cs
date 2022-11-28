using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisDemo.Extensions
{
    public static class DistributedCasheExtensions
    {
        /// <summary>
        /// This method used for data saving in redis server
        /// </summary>
        /// <typeparam name="T">Data Type, This will be calss or json or any type of data</typeparam>
        /// <param name="cache">Extenstion method parameter</param>
        /// <param name="recordId">unique id for every record</param>
        /// <param name="data">saved data on database</param>
        /// <param name="absoluteExpireTime">specifie value will be</param>
        /// <param name="unusedExpiereTime">specifie when expired the time</param>
        /// <returns></returns>
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpiereTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unusedExpiereTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T?> GetReocrdAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

    }
}
