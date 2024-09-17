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
internal class CreateBasketHandler(IBasketRepository repository)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(
        CreateBasketCommand command, CancellationToken cancellationToken)
    {
        // Save cart to database
        var username = await repository.CreateBasket(command.Cart, cancellationToken);

        // Return CreateBasketResult result
        return new CreateBasketResult(username);
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
    }
}