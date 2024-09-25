namespace eCommerceApp.Basket.API.Basket;

/// <summary>
/// Command object
/// </summary>
/// <param name="BasketCheckoutDto"></param>
public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto)
    : ICommand<CheckoutBasketResult>;

/// <summary>
/// Result object
/// </summary>
/// <param name="IsSuccess"></param>
public record CheckoutBasketResult(bool IsSuccess);

internal class CheckoutBasketHandler
    (IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.BasketCheckoutDto.Username, cancellationToken);
        if (basket == null) return new CheckoutBasketResult(false);

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasket(command.BasketCheckoutDto.Username, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}

/// <summary>
/// Validation object to be handled by MediatR pipeline
/// </summary>
public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotEmpty().WithMessage("BasketCheckoutDto cannot be null");

        RuleFor(x => x.BasketCheckoutDto.Username).NotEmpty().WithMessage("Username is required");
    }
}
