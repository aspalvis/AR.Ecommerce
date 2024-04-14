using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart ShoppingCart);

    public class GetBasketQueryHandler(IBasketRepository repository)
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            ShoppingCart? shoppingCart = await repository.GetBasket(query.Username, cancellationToken);
            return new GetBasketResult(shoppingCart);
        }
    }
}
