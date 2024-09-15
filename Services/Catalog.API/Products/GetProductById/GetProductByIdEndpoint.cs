namespace eCommerceApp.Catalog.API.Products.GetProductById;

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
            async ([FromRoute] Guid id, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            GetProductByIdQuery query = new GetProductByIdRequest(id).Adapt<GetProductByIdQuery>();
            GetProductByIdResult result = await sender.Send(query);
            GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();
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