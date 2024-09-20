namespace eCommerceApp.Catalog.API.Products.GetProductsByCategoryId;

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
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public record GetProductsByCategoryIdQuery(Guid Id, int? PageNumber = 1, int? PageSize = 10)
    : IQuery<GetProductsByCategoryIdResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Products"></param>
public record GetProductsByCategoryIdResult(IEnumerable<Product> Products);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
internal class GetProductsByCategoryIdHandler(IDocumentSession session)
    : IQueryHandler<GetProductsByCategoryIdQuery, GetProductsByCategoryIdResult>
{
    public async Task<GetProductsByCategoryIdResult> Handle(
        GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
    {
        var products = await session
            .Query<Product>()
            .Where(p => p.CategoryIds.Contains(query.Id))
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        // Return GetProductsByCategoryIdResult result
        return new GetProductsByCategoryIdResult(products);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class GetProductsByCategoryIdQueryValidator : AbstractValidator<GetProductsByCategoryIdQuery>
{
    public GetProductsByCategoryIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}