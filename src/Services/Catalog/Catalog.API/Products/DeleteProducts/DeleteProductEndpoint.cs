namespace Catalog.API.Products.DeleteProducts
{
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                DeleteProductResult result = await sender.Send(command);

                return Results.Ok(result.Adapt<DeleteProductResponse>());
            })
                .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete product by id")
                .WithDescription("Delete product by id");
        }
    }
}
