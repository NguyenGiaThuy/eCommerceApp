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
/// <param name="Unit"></param>
public record GetProductsRequest(Unit Unit); // Use Unit to represent empty request body/route/query

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
        app.MapGet("/products", async (ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            GetProductsQuery query = new GetProductsRequest(new Unit()).Adapt<GetProductsQuery>();
            GetProductsResult result = await sender.Send(query);
            GetProductsResponse response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}