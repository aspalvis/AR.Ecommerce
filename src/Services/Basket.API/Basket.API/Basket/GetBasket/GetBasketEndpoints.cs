namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart ShoppingCart);
    public class GetBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{username}", async (string username, ISender sender, CancellationToken cancellationToken = default) =>
            {
                var result = await sender.Send(new GetBasketQuery(username), cancellationToken);

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            });
        }
    }
}
