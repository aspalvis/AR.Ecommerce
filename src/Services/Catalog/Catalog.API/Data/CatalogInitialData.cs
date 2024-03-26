using Catalog.API.Data.Seed;
using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            session.Store(ProductSeed.GetProducts());
            await session.SaveChangesAsync(cancellation);
        }
    }
}
