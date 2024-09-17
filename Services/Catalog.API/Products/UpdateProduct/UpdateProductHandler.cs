namespace eCommerceApp.Catalog.API.Products.UpdateProduct;

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
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Categories"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Categories,
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
        product.Categories = command.Categories;
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

        RuleFor(x => x.Categories)
        .NotEmpty().WithMessage("Categories are required")
        .Must(xc => xc.All(c => c.Length >= 2 && c.Length <= 50))
        .WithMessage("Category must be between 2 and 50 characters long");

        RuleFor(x => x.Description)
        .NotEmpty().WithMessage("Description is required")
        .Length(2, 200).WithMessage("Description must be between 2 and 200 characters long");

        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}