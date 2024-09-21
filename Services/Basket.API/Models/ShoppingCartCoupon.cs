namespace eCommerceApp.Basket.API.Models;

public class ShoppingCartCoupon
{
    public Guid Id { get; set; } = default!;
    public string Description { get; set; } = "No discount";
    public decimal Amount { get; set; } = default!;
}
