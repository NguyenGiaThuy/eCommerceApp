namespace eCommerceApp.Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken);
    Task<string> CreateBasket(ShoppingCart cart, CancellationToken cancellationToken);
    Task<bool> DeleteBasket(string username, CancellationToken cancellationToken);
}