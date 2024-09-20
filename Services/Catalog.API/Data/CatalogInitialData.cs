
namespace eCommerceApp.Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellationToken)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellationToken)) return;

        var (products, ProductCategories) = GetPreconfiguredData(100);
        session.Store(products);
        session.Store(ProductCategories);
        await session.SaveChangesAsync(cancellationToken);
    }

    private static (IEnumerable<Product>, IEnumerable<ProductCategory>) GetPreconfiguredData(int productsCount)
    {
        var random = new Random();
        var names = new[] { "Iphone X", "Samsung S21", "Google Pixel 5", "OnePlus 9", "Xiaomi Mi 11" };
        var categoryDict = new Dictionary<Guid, string>
        {
            { Guid.NewGuid(), "Smart Phone" },
            { Guid.NewGuid(), "Camera" },
            { Guid.NewGuid(), "Electronics" },
            { Guid.NewGuid(), "Mobile Device" },
        };
        var categories = categoryDict
            .Select(kvp => new ProductCategory
            {
                Id = kvp.Key,
                Name = kvp.Value
            }).ToList();

        var products = new List<Product>();


        for (int i = 0; i < productsCount; i++)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = names[random.Next(names.Length)],
                Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = $"product-{i + 1}.png",
                Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                CategoryIds = new List<Guid>()
            };

            var categoriesCount = random.Next(1, categoryDict.Count);
            int j = 0;
            while (j < categoriesCount)
            {
                var categoryId = categoryDict.ToList()[random.Next(categoryDict.Count)].Key;
                if (!product.CategoryIds.Contains(categoryId))
                {
                    product.CategoryIds.Add(categoryId);
                    j++;
                }
            }

            products.Add(product);
        }

        return (products, categories);
    }
}
