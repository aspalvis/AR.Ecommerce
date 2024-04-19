using Basket.API.Data;
using Discount.gRPC;

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

    public class StoreBasketHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.ShoppingCart, cancellationToken);

            ShoppingCart? cart = await repository.StoreBasket(command.ShoppingCart, cancellationToken);

            return new StoreBasketResult(cart.Username);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (ShoppingCartItem item in cart.Items)
            {
                CouponModel? coupon = await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);

                if (coupon is not null)
                {
                    item.Price -= coupon.Amount;
                }
            }
        }
    }
}
