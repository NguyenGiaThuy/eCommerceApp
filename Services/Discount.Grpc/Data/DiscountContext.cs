
namespace eCommerceApp.DiscountGrpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;

    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasKey(x => x.ProductId);
        modelBuilder.Entity<Coupon>().HasData(GetPreconfiguredData(10));
    }

    private static IEnumerable<Coupon> GetPreconfiguredData(int couponsCount)
    {
        var random = new Random();
        var productDict = new Dictionary<string, (string, decimal)>
        {
            { "Iphone X", ("Iphone discount", 50) },
            { "Samsung S21", ("Samsung discount", 100) },
            { "Google Pixel 5", ("Google Pixel discount", 70) },
            { "OnePlus 9", ("OnePlus discount", 80) },
            { "Xiaomi Mi 11", ("Xiaomi discount", 30) }
        };

        var coupons = new List<Coupon>();
        for (int i = 0; i < couponsCount; i++)
        {
            var productName = productDict.ToList()[random.Next(productDict.Count)].Key;
            var coupon = new Coupon
            {
                ProductId = Guid.NewGuid(),
                ProductName = productName,
                Description = productDict[productName].Item1,
                Amount = productDict[productName].Item2
            };

            coupons.Add(coupon);
        }

        return coupons;
    }
}