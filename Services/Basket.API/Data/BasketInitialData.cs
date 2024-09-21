
namespace eCommerceApp.Basket.API.Data;

public class BasketInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellationToken)
    {
        using var session = store.LightweightSession();

        if (await session.Query<ShoppingCart>().AnyAsync(cancellationToken)) return;

        session.Store(GetPreconfiguredData());
        await session.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<ShoppingCart> GetPreconfiguredData()
    {
        var random = new Random();
        var usernames = new[] { "binh", "kiet", "thinh", "thuy" };
        var colors = new[] { "Red", "Blue", "White", "Black" };

        return new List<ShoppingCart>
        {
            new ShoppingCart
            {
                Username = usernames[random.Next(usernames.Length)],
                Items = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ProductId = new Guid("16676e61-9c66-4f5d-9ba2-a775b6d326d1"),
                        ProductName = "Iphone X",
                        Quantity = random.Next(1, 4),
                        Color = colors[random.Next(colors.Length)],
                        Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                        Coupon = new ShoppingCartCoupon()
                    },
                    new ShoppingCartItem
                    {
                        ProductId = new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"),
                        ProductName = "Samsung S21",
                        Quantity = random.Next(1, 4),
                        Color = colors[random.Next(colors.Length)],
                        Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                        Coupon = new ShoppingCartCoupon()
                    }
                }
            },
            new ShoppingCart
            {
                Username = usernames[random.Next(usernames.Length)],
                Items = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ProductId = new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"),
                        ProductName = "Google Pixel 5",
                        Quantity = random.Next(1, 4),
                        Color = colors[random.Next(colors.Length)],
                        Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                        Coupon = new ShoppingCartCoupon()
                    },
                    new ShoppingCartItem
                    {
                        ProductId = new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"),
                        ProductName = "OnePlus 9",
                        Quantity = random.Next(1, 4),
                        Color = colors[random.Next(colors.Length)],
                        Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                        Coupon = new ShoppingCartCoupon()
                    }
                }
            },
            new ShoppingCart
            {
                Username = usernames[random.Next(usernames.Length)],
                Items = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ProductId = new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"),
                        ProductName = "Samsung S21",
                        Quantity = random.Next(1, 4),
                        Color = colors[random.Next(colors.Length)],
                        Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                        Coupon = new ShoppingCartCoupon()
                    },
                    new ShoppingCartItem
                    {
                        ProductId = new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"),
                        ProductName = "OnePlus 9",
                        Quantity = random.Next(1, 4),
                        Color = colors[random.Next(colors.Length)],
                        Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                        Coupon = new ShoppingCartCoupon()
                    },
                    new ShoppingCartItem
                    {
                        ProductId = new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"),
                        ProductName = "Google Pixel 5",
                        Quantity = random.Next(1, 4),
                        Color = colors[random.Next(colors.Length)],
                        Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                        Coupon = new ShoppingCartCoupon()
                    }
                }
            }
        };
    }
}
