using Application.Context.Pocos;
using Common.Entity;
using Common.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class TopPoCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IServiceProvider _serviceProvider;
        private const string CacheKey = "TopPurchaseOrders";

        public TopPoCacheService(IDistributedCache cache, IServiceProvider serviceProvider)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<PoEntity>> GetTop7PosAsync()
        {
            var cachedData = await _cache.GetStringAsync(CacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                //_cache.Remove(CacheKey);
                var result = JsonSerializer.Deserialize<List<PurchaseOrder>>(cachedData)!;
                return result.Select(e => e.GetPoEntity()).Select(e => e.Value).ToList();
            }

            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<PocoRepository>();
            var topPos = (await repository.GetTopPurchaseOrdersAsync(7)).ToList();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = false
            };
            var jsonData = JsonSerializer.Serialize(topPos, options);
            await _cache.SetStringAsync(CacheKey, jsonData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return topPos.Select(e => e.GetPoEntity()).Select(e => e.Value).ToList(); ;
        }
    }


}
