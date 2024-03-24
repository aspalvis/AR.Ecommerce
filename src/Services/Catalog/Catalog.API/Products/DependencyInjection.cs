using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.DeleteProducts;
using Catalog.API.Products.GetProductByCategory;
using Catalog.API.Products.GetProducts;
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products
{
    public static class DependencyInjection
    {
        public static void WithProductEndpoints(this CarterConfigurator carterConfigurator)
        {
            carterConfigurator.WithModule<CreateProductEndpoint>();
            carterConfigurator.WithModule<GetProductsEndpoint>();
            carterConfigurator.WithModule<GetProductByIdEndpoint>();
            carterConfigurator.WithModule<GetProductByCategoryEndpoint>();
            carterConfigurator.WithModule<UpdateProductEndpoint>();
            carterConfigurator.WithModule<DeleteProductEndpoint>();
        }
    }
}
