
namespace eCommerceApp.Basket.API.Basket;

/// <summary>
/// Command object
/// </summary>
/// <param name="BasketCheckoutDto"></param>
public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record CheckoutBasketResponse(bool IsSuccess);

/// <summary>
/// Repository object
/// </summary>
public class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout",
            async (CheckoutBasketRequest request, ISender sender) =>
        {
            // Pipeline: Request -> Command -> Result -> Response
            var command = request.Adapt<CheckoutBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CheckoutBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("CheckoutBasket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Checkout Basket")
        .WithDescription("Checkout Basket");
    }
}