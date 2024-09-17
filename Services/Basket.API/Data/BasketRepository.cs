
namespace eCommerceApp.Basket.API.Data;

public class BasketRepository(IDocumentSession session)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken)
    {
        var cart = await session.LoadAsync<ShoppingCart>(username, cancellationToken);
        if (cart == null) throw new BasketNotFoundException(username);
        return cart;
    }

    public async Task<string> CreateBasket(ShoppingCart cart, CancellationToken cancellationToken)
    {
        session.Store(cart);
        await session.SaveChangesAsync(cancellationToken);
        return cart.Username;
    }

    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken)
    {
        session.Delete<ShoppingCart>(username);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }
}