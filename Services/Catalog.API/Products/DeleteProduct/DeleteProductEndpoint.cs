namespace eCommerceApp.Catalog.API.Products.DeleteProduct;

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
/// Command object
/// </summary>
/// <param name="Id"></param>
public record DeleteProductRequest(Guid Id);

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record DeleteProductResponse(bool IsSuccess);

/// <summary>
/// Repository object
/// </summary>
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}",
            async ([FromRoute] Guid id, ISender sender) =>
        {
            // Pipeline: Request -> Command -> Result -> Response
            DeleteProductCommand command = new DeleteProductRequest(id).Adapt<DeleteProductCommand>();
            DeleteProductResult result = await sender.Send(command);
            DeleteProductResponse response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");
    }
}