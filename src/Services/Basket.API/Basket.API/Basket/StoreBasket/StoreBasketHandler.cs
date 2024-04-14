using Basket.API.Data;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string Username);

    public class StoreBasletCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasletCommandValidator()
        {
            RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.ShoppingCart.Username).NotEmpty().WithMessage("Username is required");
        }
    }

    public class StoreBasketHandler(IBasketRepository repository)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart? cart = await repository.StoreBasket(command.ShoppingCart, cancellationToken);

            return new StoreBasketResult(cart.Username);
        }
    }
}
