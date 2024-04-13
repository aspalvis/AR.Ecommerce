namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart ShoppingCart);

    public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            return new GetBasketResult(new ShoppingCart("he"));
        }
    }
}
