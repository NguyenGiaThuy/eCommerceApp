namespace eCommerceApp.Basket.API.Basket.GetBasket;

/*
Internal architecture: Vertical slice
    1. UI layer (or Presentation layer or API layer)
    2. Application layer - current
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
public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="Cart"></param>
public record GetBasketResult(ShoppingCart Cart);

/// <summary>
/// Repository object
/// </summary>
/// <param name="repository"></param>
internal class GetBasketHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(
        GetBasketQuery query, CancellationToken cancellationToken)
    {
        // Get cart from database
        var cart = await repository.GetBasket(query.Username, cancellationToken);

        // Return GetBasketResult result
        return new GetBasketResult(cart);
    }
}