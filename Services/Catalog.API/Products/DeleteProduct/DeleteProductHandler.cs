namespace eCommerceApp.Catalog.API.Products.DeleteProduct;

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
/// Query object
/// </summary>
/// <param name="Id"></param>
public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record DeleteProductResult(bool IsSuccess);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
/// <param name="logger"></param>
internal class DeleteProductHandler(IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(
        DeleteProductCommand command, CancellationToken cancellationToken)
    {
        // Delete product from database
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);

        // Return GetProductByIdResult result
        return new DeleteProductResult(true);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}