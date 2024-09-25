namespace eCommerceApp.Basket.API.Basket.CreateBasket;

/// <summary>
/// Command object
/// </summary>
/// <param name="Cart"></param>
public record CreateBasketRequest(ShoppingCart Cart);

/// <summary>
/// Result object
/// </summary>
/// <param name="Username"></param>
public record CreateBasketResponse(string Username);

/// <summary>
/// Repository object
/// </summary>
public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket",
            async (CreateBasketRequest request, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            var command = request.Adapt<CreateBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateBasketResponse>();
            return Results.Created($"/basket/{response.Username}", response);
        })
        .WithName("CreateBasket")
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Basket")
        .WithDescription("Create Basket");
    }
}