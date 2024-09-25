namespace eCommerceApp.Catalog.API.Products.GetProductById;

/// <summary>
/// Query object
/// </summary>
/// <param name="Id"></param>
public record GetProductByIdRequest(Guid Id);

/// <summary>
/// Result object
/// </summary>
/// <param name="Product"></param>
public record GetProductByIdResponse(Product Product);

/// <summary>
/// Repository object
/// </summary>
public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}",
            async (Guid id, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            var query = new GetProductByIdRequest(id).Adapt<GetProductByIdQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}