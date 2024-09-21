namespace eCommerceApp.Discount.Grpc.Models;

public class Coupon
{
    public Guid Id { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
}
