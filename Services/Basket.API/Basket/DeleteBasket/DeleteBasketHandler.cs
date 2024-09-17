namespace eCommerceApp.Basket.API.Basket.DeleteBasket;

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
/// <param name="Username"></param>
public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record DeleteBasketResult(bool IsSuccess);

/// <summary>
/// Repository object
/// </summary>
/// <param name="repository"></param>
internal class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(
        DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        // Delete cart from database
        bool isSuccess = await repository.DeleteBasket(command.Username, cancellationToken);

        // Return GetProductsResult result
        return new DeleteBasketResult(isSuccess);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
    }
}