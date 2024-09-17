namespace eCommerceApp.Basket.API.Basket.GetBasket;

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