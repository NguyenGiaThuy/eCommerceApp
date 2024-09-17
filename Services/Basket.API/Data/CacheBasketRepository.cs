namespace eCommerceApp.Basket.API.Data;

/// <summary>
/// Acts as a decorator object extends IBasketRepository interface capability (decorator pattern)
/// </summary>
/// <param name="repository">Acts as a proxy object (proxy pattern)</param>
/// <param name="cache"></param>
public class CacheBasketRepository
    (IBasketRepository repository, IDistributedCache cache)
    : IBasketRepository
{
    private string CacheKey(string id) => $"basket:{id}";
    private int expirationTime = 1;

    public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken)
    {
        var cachedBasket = await cache.GetStringAsync(CacheKey(username), cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var cart = await repository.GetBasket(username, cancellationToken);
        await cache.SetStringAsync(
            CacheKey(username),
            JsonSerializer.Serialize(cart),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(expirationTime)
            },
            cancellationToken);

        return cart;
    }

    public async Task<string> CreateBasket(ShoppingCart cart, CancellationToken cancellationToken)
    {
        var username = await repository.CreateBasket(cart, cancellationToken);

        await cache.SetStringAsync(
           CacheKey(username),
           JsonSerializer.Serialize(cart),
           new DistributedCacheEntryOptions
           {
               AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(expirationTime)
           },
           cancellationToken);

        return username;
    }

    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(username, cancellationToken);
        await cache.RemoveAsync(CacheKey(username), cancellationToken);
        return true;
    }
}