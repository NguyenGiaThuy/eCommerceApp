namespace eCommerceApp.Catalog.API.Products.UpdateProduct;

/// <summary>
/// Command object
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="CategoryIds"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record UpdateProductRequest(
    Guid Id,
    string Name,
    List<Guid> CategoryIds,
    string Description,
    string ImageFile,
    decimal Price
);

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record UpdateProductResponse(bool IsSuccess);

/// <summary>
/// Repository object
/// </summary>
public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products",
            async (UpdateProductRequest request, ISender sender) =>
        {
            // Pipeline: Request -> command -> Result -> Response
            var command = request.Adapt<UpdateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Product")
        .WithDescription("Update Product");
    }
}