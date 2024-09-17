namespace eCommerceApp.Basket.API.Basket.DeleteBasket;

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
/// <param name="Username"></param>
public record DeleteBasketRequest(string Username);

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record DeleteBasketResponse(bool IsSuccess);

/// <summary>
/// Repository object
/// </summary>
public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}",
            async (string username, ISender sender) =>
        {
            // Pipeline: Request -> Query -> Result -> Response
            var command = new DeleteBasketRequest(username).Adapt<DeleteBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket");
    }
}