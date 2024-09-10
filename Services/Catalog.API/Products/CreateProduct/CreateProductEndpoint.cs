namespace eCommerceApp.Catalog.API.Products.CreateProduct;

/*
Internal architecture: Vertical slice
    1. UI layer (or Presentation layer or API layer) - current
    2. Application layer
    3. Domain layer
    4. Infrastructure layer
Design pattern/Principle: CQRS handler
    1. Command query object
    2. Result object
    3. Repository object
*/

/// <summary>
/// Command query object
/// </summary>
/// <param name="Name"></param>
/// <param name="Categories"></param>
/// <param name="Description"></param>
/// <param name="ImageFile"></param>
/// <param name="Price"></param>
public record CreateProductRequest(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price);

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
            CreateProductCommand command = request.Adapt<CreateProductCommand>();
            CreateProductResult result = await sender.Send(command);
            CreateProductResponse response = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}