
namespace eCommerceApp.DiscountGrpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;

    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(GetPreconfiguredData(10));
    }

    private static IEnumerable<Coupon> GetPreconfiguredData(int couponsCount)
    {
        // var random = new Random();
        // var productDict = new Dictionary<string, (string, decimal)>
        // {
        //     { "Iphone X", ("Iphone discount", 50) },
        //     { "Samsung S21", ("Samsung discount", 100) },
        //     { "Google Pixel 5", ("Google Pixel discount", 70) },
        //     { "OnePlus 9", ("OnePlus discount", 80) },
        //     { "Xiaomi Mi 11", ("Xiaomi discount", 30) }
        // };

        // var coupons = new List<Coupon>();
        // for (int i = 0; i < couponsCount; i++)
        // {
        //     var productName = productDict.ToList()[random.Next(productDict.Count)].Key;
        //     var coupon = new Coupon
        //     {
        //         Id = Guid.NewGuid(),
        //         ProductId = Guid.NewGuid(),
        //         ProductName = productName,
        //         Description = productDict[productName].Item1,
        //         Amount = productDict[productName].Item2
        //     };

        //     coupons.Add(coupon);
        // }

        // return coupons;

        return new List<Coupon>
        {
            new Coupon
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("16676e61-9c66-4f5d-9ba2-a775b6d326d1"),
                ProductName = "Iphone X",
                Description = "Iphone discount",
                Amount = 50
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("c63d2f83-f353-42e1-a07f-8be599379a32"),
                ProductName = "Samsung S21",
                Description = "Samsung discount",
                Amount = 100
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("1ad04d68-4871-47af-a2bb-7f6864cefc08"),
                ProductName = "Google Pixel 5",
                Description = "Google Pixel discount",
                Amount = 70
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("70098a3c-6917-49b9-910c-8078cd7fdf21"),
                ProductName = "OnePlus 9",
                Description = "OnePlus discount",
                Amount = 60
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                ProductId = new Guid("6967eb61-fb97-4791-a808-7c3c0208cd70"),
                ProductName = "Xiaomi Mi 11",
                Description = "Xiami discount",
                Amount = 15
            }
        };
    }
}