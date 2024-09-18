namespace eCommerceApp.Catalog.API.Products.GetProductsByCategory;

/*
Internal architecture: Vertical slice
    1. UI layer (or Presentation layer or API layer) - current
    2. Application layer
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
public record GetProductsByCategoryRequest(Guid Id, int? PageNumber = 1, int? PageSize = 10);

/// <summary>
/// Result object
/// </summary>
/// <param name="Products"></param>
public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

/// <summary>
/// Repository object
/// </summary>
public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{id}",
            async (Guid id, [AsParameters] GetProductsByCategoryRequest request, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            var query = request.Adapt<GetProductsByCategoryQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsByCategoryResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Category")
        .WithDescription("Get Products By Category");
    }
}