namespace eCommerceApp.Basket.API.Basket.GetBasket;

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