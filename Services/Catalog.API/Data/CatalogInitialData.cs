
using Marten.Linq.SoftDeletes;

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

        // var products = new List<Product>();
        // for (int i = 0; i < productsCount; i++)
        // {
        //     var product = new Product
        //     {
        //         Id = Guid.NewGuid(),
        //         Name = names[random.Next(names.Length)],
        //         Description = "This phone is the company's biggest change to its flagship smartphone in years.",
        //         ImageFile = $"product-{i + 1}.png",
        //         Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
        //         CategoryIds = new List<Guid>()
        //     };

        //     var categoriesCount = random.Next(1, categoryDict.Count);
        //     int j = 0;
        //     while (j < categoriesCount)
        //     {
        //         var categoryId = categoryDict.ToList()[random.Next(categoryDict.Count)].Key;
        //         if (!product.CategoryIds.Contains(categoryId))
        //         {
        //             product.CategoryIds.Add(categoryId);
        //             j++;
        //         }
        //     }

        //     products.Add(product);
        // }

        // return (products, categories);

        return (new List<Product>
        {
            new Product
            {
                Id = new Guid("16676e61-9c66-4f5d-9ba2-a775b6d326d1"),
                Name = "Iphone X",
                Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = $"product-1.png",
                Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                CategoryIds = new List<Guid>() { categories[0].Id, categories[2].Id }
            },
            new Product
            {
                Id = new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"),
                Name = "Samsung S21",
                Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = $"product-2.png",
                Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                CategoryIds = new List<Guid>() { categories[1].Id, categories[3].Id }
            },
            new Product
            {
                Id = new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"),
                Name = "Google Pixel 5",
                Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = $"product-3.png",
                Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                CategoryIds = new List<Guid>() { categories[1].Id, categories[0].Id }
            },
            new Product
            {
                Id = new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"),
                Name = "OnePlus 9",
                Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = $"product-4.png",
                Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                CategoryIds = new List<Guid>() { categories[0].Id, categories[2].Id }
            },
            new Product
            {
                Id = new Guid("6967eb61-fb97-4791-a808-7c3c0208cd70"),
                Name = "Xiaomi 11",
                Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = $"product-5.png",
                Price = (decimal)Math.Round(random.NextDouble() * (999.99 - 299.99) + 299.99, 2),
                CategoryIds = new List<Guid>() { categories[0].Id, categories[3].Id }
            }
        }, categories);
    }
}
