
namespace eCommerceApp.Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellationToken)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellationToken)) return;

        session.Store(GetPreconfiguredProducts(100));
        await session.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts(int productsCount)
    {
        var random = new Random();
        var names = new[] { "Iphone X", "Samsung S21", "Google Pixel 5", "OnePlus 9", "Xiaomi Mi 11" };
        var categories = new[] { "Smart Phone", "Camera", "Electronics", "Mobile Device" };

        var products = new List<Product>();
        for (int i = 0; i < productsCount; i++)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = names[random.Next(names.Length)],
                Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = $"product-{i + 1}.png",
                Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                Categories = new List<string>()
            };

            var categoriesCount = random.Next(1, categories.Length);
            int j = 0;
            while (j < categoriesCount)
            {
                var category = categories[random.Next(categories.Length)];
                if (!product.Categories.Contains(category))
                {
                    product.Categories.Add(category);
                    j++;
                }
            }

            products.Add(product);
        }

        return products;
    }
}
