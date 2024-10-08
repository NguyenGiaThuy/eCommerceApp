namespace eCommerceApp.Catalog.API.Products.GetProductById;

/// <summary>
/// Query object
/// </summary>
/// <param name="Id"></param>
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Product"></param>
public record GetProductByIdResult(Product Product);

/// <summary>
/// Repository object
/// </summary>
/// <param name="session"></param>
internal class GetProductByIdHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(
        GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        // Get product by id from database
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
        if (product == null) throw new ProductNotFoundException(query.Id);

        // Return GetProductByIdResult result
        return new GetProductByIdResult(product);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}