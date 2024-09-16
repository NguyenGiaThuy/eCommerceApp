namespace eCommerceApp.Catalog.API.Products.GetProducts;

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
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);

/// <summary>
/// Result object
/// </summary>
/// <param name="Products"></param>
public record GetProductsResponse(IEnumerable<Product> Products);

/// <summary>
/// Repository object
/// </summary>
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",
            async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            var query = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}