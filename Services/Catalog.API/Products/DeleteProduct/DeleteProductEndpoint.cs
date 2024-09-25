namespace eCommerceApp.Catalog.API.Products.DeleteProduct;

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
            async (Guid id, ISender sender) =>
        {
            // Pipeline: Request -> Command -> Result -> Response
            var command = new DeleteProductRequest(id).Adapt<DeleteProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteProductResponse>();
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