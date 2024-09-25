namespace eCommerceApp.Catalog.API.Products.GetProducts;

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