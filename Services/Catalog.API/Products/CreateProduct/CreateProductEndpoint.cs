namespace eCommerceApp.Catalog.API.Products.CreateProduct;

/// <summary>
/// Command object
/// </summary>
/// <param name="Name"></param>
/// <param name="CategoryIds"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record CreateProductRequest(
    string Name,
    List<Guid> CategoryIds,
    string Description,
    string ImageFile,
    decimal Price
);

/// <summary>
/// Result object
/// </summary>
/// <param name="Id"></param>
public record CreateProductResponse(Guid Id);

/// <summary>
/// Repository object
/// </summary>
public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
            async (CreateProductRequest request, ISender sender) =>
        {
            // Pipeline: Request -> Command -> Result -> Response
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}