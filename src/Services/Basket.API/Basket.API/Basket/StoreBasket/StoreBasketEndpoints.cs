
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart ShoppingCart);
    public record StoreBasketResponse(string Username);

    public class StoreBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                StoreBasketCommand command = request.Adapt<StoreBasketCommand>();
                StoreBasketResult result = await sender.Send(command);
                StoreBasketResponse response = result.Adapt<StoreBasketResponse>();

                return Results.Created($"/basket/{response.Username}", response);
            })
                .WithName("CreateProduct")
                .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
