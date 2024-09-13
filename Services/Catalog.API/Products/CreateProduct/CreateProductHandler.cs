using eCommerceApp.BuildingBlocks.CQRS;
using eCommerceApp.Catalog.API.Models;

namespace eCommerceApp.Catalog.API.Products.CreateProduct;

/*
Internal architecture: Vertical slice
    1. UI layer (or Presentation layer or API layer)
    2. Application layer - current
    3. Domain layer
    4. Infrastructure layer
Design pattern/Principle: CQRS handler
    1. Command query object
    2. Result object
    3. Repository object
*/

/// <summary>
/// Command query object
/// </summary>
/// <param name="Name"></param>
/// <param name="Categories"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record CreateProductCommand(
    string Name,
    List<string> Categories,
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
internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create Product entity from command object
        Product product = new Product
        {
            Name = command.Name,
            Categories = command.Categories,
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