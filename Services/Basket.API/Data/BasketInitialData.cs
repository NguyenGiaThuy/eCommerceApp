
namespace eCommerceApp.Basket.API.Data;

public class BasketInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellationToken)
    {
        using var session = store.LightweightSession();

        if (await session.Query<ShoppingCart>().AnyAsync(cancellationToken)) return;

        session.Store(GetPreconfiguredData(10));
        await session.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<ShoppingCart> GetPreconfiguredData(int shoppingCartsCount)
    {
        var random = new Random();
        var usernames = new[] { "binh", "kiet", "thinh", "thuy" };
        var productNames = new[] { "Iphone X", "Samsung S21", "Google Pixel 5", "OnePlus 9", "Xiaomi Mi 11" };
        var colors = new[] { "Red", "Blue", "White", "Black" };

        var shoppingCarts = new List<ShoppingCart>();
        for (int i = 0; i < shoppingCartsCount; i++)
        {
            var shoppingCart = new ShoppingCart
            {
                Username = usernames[random.Next(usernames.Length)],
                Items = new List<ShoppingCartItem>()
            };

            var itemsCount = random.Next(1, productNames.Length);
            for (int j = 0; j < itemsCount; j++)
            {
                var item = new ShoppingCartItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = productNames[random.Next(productNames.Length)],
                    Quantity = random.Next(1, 4),
                    Color = colors[random.Next(colors.Length)],
                    Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2)
                };

                shoppingCart.Items.Add(item);
            }

            shoppingCarts.Add(shoppingCart);
        }

        return shoppingCarts;
    }
}
