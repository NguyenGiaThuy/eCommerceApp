namespace eCommerceApp.Catalog.API.Products.CreateProduct;

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
/// <param name="Name"></param>
/// <param name="CategoryIds"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record CreateProductCommand(
    string Name,
    List<Guid> CategoryIds,
    string Description,
    string ImageFile,
    decimal Price)
    : ICommand<CreateProductResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Id"></param>
public record CreateProductResult(Guid Id);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
internal class CreateProductHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(
        CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create Product entity from command object
        var product = new Product
        {
            Name = command.Name,
            CategoryIds = command.CategoryIds,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        // Save to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // Return CreateProductResult result
        return new CreateProductResult(product.Id);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required")
        .Length(2, 100).WithMessage("Name must be between 2 and 100 characters long");

        RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("Category ids are required");

        RuleFor(x => x.Description)
        .NotEmpty().WithMessage("Description is required")
        .Length(2, 200).WithMessage("Description must be between 2 and 200 characters long");

        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}