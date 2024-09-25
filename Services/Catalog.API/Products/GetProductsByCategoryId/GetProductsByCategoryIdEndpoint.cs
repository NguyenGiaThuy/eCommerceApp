namespace eCommerceApp.Catalog.API.Products.GetProductsByCategoryId;

/// <summary>
/// Query object
/// </summary>
/// <param name="Id"></param>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public record GetProductsByCategoryIdRequest
    (Guid Id, int? PageNumber = 1, int? PageSize = 10);

/// <summary>
/// Result object
/// </summary>
/// <param name="Products"></param>
public record GetProductsByCategoryIdResponse(IEnumerable<Product> Products);

/// <summary>
/// Repository object
/// </summary>
public class GetProductsByCategoryIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{id}",
            async (Guid id, [AsParameters] GetProductsByCategoryIdRequest request, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            var query = request.Adapt<GetProductsByCategoryIdQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsByCategoryIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductsByCategoryId")
        .Produces<GetProductsByCategoryIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Products By Category Id")
        .WithDescription("Get Products By Category Id");
    }
}