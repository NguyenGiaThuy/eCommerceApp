namespace eCommerceApp.Catalog.API.Products.GetProductsByCategory;

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
/// <param name="Category"></param>
public record GetProductsByCategoryQuery(string Category, int? PageNumber = 1, int? PageSize = 10)
    : IQuery<GetProductsByCategoryResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Products"></param>
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
internal class GetProductsByCategoryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(
        GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        // // Get products from database by category
        // var products = await session
        //     .Query<Product>()
        //     .Where(p => p.Categories.Contains(query.Category))
        //     .ToListAsync(cancellationToken);
        var products = await session
            .Query<Product>()
            .Where(p => p.Categories.Contains(query.Category))
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        // Return GetProductsByCategoryResult result
        return new GetProductsByCategoryResult(products);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class GetProductsByCategoryQueryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    public GetProductsByCategoryQueryValidator()
    {
        RuleFor(x => x.Category)
        .NotEmpty().WithMessage("Category is required")
        .Length(2, 50).WithMessage("Category must be between 2 and 50 characters long");
    }
}