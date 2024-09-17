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
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Products"></param>
public record GetProductsResult(IEnumerable<Product> Products);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
internal class GetProductsHandler(IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(
        GetProductsQuery query, CancellationToken cancellationToken)
    {
        // Get products from database
        var products = await session
            .Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        // Return GetProductsResult result
        return new GetProductsResult(products);
    }
}