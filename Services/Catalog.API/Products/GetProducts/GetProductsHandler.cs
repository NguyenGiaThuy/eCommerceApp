namespace eCommerceApp.Catalog.API.Products.GetProducts;

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
/// <param name="Unit"></param>
public record GetProductsQuery(Unit Unit) : IQuery<GetProductsResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Products"></param>
public record GetProductsResult(IEnumerable<Product> Products);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
/// <param name="logger"></param>
internal class GetProductsHandler
    (IDocumentSession session, ILogger<GetProductsHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // Log information
        logger.LogInformation($"GetProductsHandler.Handle called with {query}");

        // Get products from database
        IEnumerable<Product> products = await session.Query<Product>().ToListAsync(cancellationToken);

        // Return GetProductsResult result
        return new GetProductsResult(products);
    }
}