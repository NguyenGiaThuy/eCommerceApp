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
/// <param name="logger"></param>
internal class UpdateProductHandler
    (IDocumentSession session, ILogger<UpdateProductHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(
        UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // Log information
        logger.LogInformation($"UpdateProductHandler.Handle called with {command}");

        // Get product by id from database
        Product product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null) throw new ProductNotFoundException();

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