namespace eCommerceApp.Catalog.API.Products.UpdateProduct;

/// <summary>
/// Command object
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="CategoryIds"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<Guid> CategoryIds,
    string Description,
    string ImageFile,
    decimal Price)
    : ICommand<UpdateProductResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record UpdateProductResult(bool IsSuccess);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
internal class UpdateProductHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(
        UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // Get product by id from database
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null) throw new ProductNotFoundException(command.Id);

        // Update product
        product.Name = command.Name;
        product.CategoryIds = command.CategoryIds;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        // Return UpdateProductResult result
        return new UpdateProductResult(true);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required")
        .Length(2, 100).WithMessage("Name must be between 2 and 100 characters long");

        RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("Categories are required");

        RuleFor(x => x.Description)
        .NotEmpty().WithMessage("Description is required")
        .Length(2, 200).WithMessage("Description must be between 2 and 200 characters long");

        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}