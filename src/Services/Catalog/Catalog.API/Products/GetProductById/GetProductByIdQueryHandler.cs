namespace Catalog.API.Products.GetProducts
{
    internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);

            Product product = await session.LoadAsync<Product>(query.Id, cancellationToken) ?? throw new ProductNotFoundException(query.Id);

            return new GetProductByIdResult(product);
        }
    }

    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
}
