namespace eCommerceApp.Basket.API.Basket.DeleteBasket;

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