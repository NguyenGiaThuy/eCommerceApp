namespace eCommerceApp.Basket.API.Basket.GetBasket;

/// <summary>
/// Query object
/// </summary>
/// <param name="Username"></param>
public record GetBasketRequest(string Username);

/// <summary>
/// Result object
/// </summary>
/// <param name="Cart"></param>
public record GetBasketResponse(ShoppingCart Cart);

/// <summary>
/// Repository object
/// </summary>
public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}",
            async (string username, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            var query = new GetBasketRequest(username).Adapt<GetBasketQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBasket")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Basket")
        .WithDescription("Get Basket");
    }
}