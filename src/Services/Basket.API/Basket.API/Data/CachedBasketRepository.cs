using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache)
        : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
        {
            bool result = await basketRepository.DeleteBasket(username, cancellationToken);

            await cache.RemoveAsync(username, cancellationToken);

            return result;
        }

        public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default)
        {
            string? cached = await cache.GetStringAsync(username, cancellationToken);

            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cached)!;
            }

            ShoppingCart? basket = await basketRepository.GetBasket(username, cancellationToken);

            await cache.SetStringAsync(username, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            ShoppingCart? stored = await basketRepository.StoreBasket(basket, cancellationToken);

            await cache.SetStringAsync(basket.Username, JsonSerializer.Serialize(stored), cancellationToken);

            return stored;
        }
    }
}
