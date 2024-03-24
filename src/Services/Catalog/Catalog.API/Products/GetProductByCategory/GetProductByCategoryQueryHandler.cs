﻿namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Product> Products);

    internal class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsByCategoryQueryHandler.Handle called with {@Query}", query);

            IEnumerable<Product> products = await session.Query<Product>()
                .Where(x => x.Category.Contains(query.Category))
                .ToListAsync(token: cancellationToken);

            return new GetProductByCategoryResult(products);
        }
    }
}
