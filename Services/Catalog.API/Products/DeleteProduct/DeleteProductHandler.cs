namespace eCommerceApp.Catalog.API.Products.DeleteProduct;

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