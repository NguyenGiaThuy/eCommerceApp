namespace eCommerceApp.Basket.API.Basket.CreateBasket;

/*
Internal architecture: Vertical slice
    1. UI layer (or Presentation layer or API layer)
    2. Application layer - current
    3. Domain layer
    4. Infrastructure layer
Design pattern/Principle: CQRS handler
    1. Command/query object
    2. Result object
    3. Repository object
*/

/// <summary>
/// Command object
/// </summary>
/// <param name="Cart"></param>
public record CreateBasketCommand(ShoppingCart Cart) : ICommand<CreateBasketResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Username"></param>
public record CreateBasketResult(string Username);

/// <summary>
/// Repository object
/// </summary>
/// <param name="repository"></param>
internal class CreateBasketHandler
    (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountClient)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(
        CreateBasketCommand command, CancellationToken cancellationToken)
    {
        // Apply discounts for all items in cart
        await ApplyDiscounts(command.Cart, cancellationToken);

        // Save cart to database
        var username = await repository.CreateBasket(command.Cart, cancellationToken);

        // Return CreateBasketResult result
        return new CreateBasketResult(username);
    }

    private async Task ApplyDiscounts(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var discountsResponse = await discountClient.GetDiscountsAsync(new GetDiscountsRequest
            { ProductId = item.ProductId.ToString() }, cancellationToken: cancellationToken);

            var couponModel = discountsResponse.Coupons.OrderBy(x => x.Amount).FirstOrDefault();
            if (couponModel != null) item.Coupon = couponModel.Adapt<ShoppingCartCoupon>();
        }
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotEmpty().WithMessage("Cart cannot be null");

        RuleFor(x => x.Cart.Username).NotEmpty().WithMessage("Username is required");

        RuleForEach(x => x.Cart.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("All item ids are required");

            item.RuleFor(x => x.ProductName).NotEmpty().WithMessage("All item names are required")
            .Length(2, 100).WithMessage("All item names must be between 2 and 100 characters long");

            item.RuleFor(x => x.Color).NotEmpty().WithMessage("All item colors are required")
            .Length(2, 30).WithMessage("All item colors must be between 2 and 30 characters long");

            item.RuleFor(x => x.Price).GreaterThan(0).WithMessage("All item prices must be greater than 0");

            item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("All item quantities must be greater than 0");
        });
    }
}