namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Request}", request);

            Product product = await session.LoadAsync<Product>(request.Id, cancellationToken) ?? throw new ProductNotFoundException();

            product.Name = request.Name;
            product.Description = request.Description;
            product.ImageFile = request.ImageFile;
            product.Price = request.Price;
            product.Category = request.Category;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
