using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductByCategory;
using Catalog.API.Products.GetProducts;
using Catalog.API.Products.UpdateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(configurator: c =>
{
    c.WithModule<CreateProductEndpoint>();
    c.WithModule<GetProductsEndpoint>();
    c.WithModule<GetProductByIdEndpoint>();
    c.WithModule<GetProductByCategoryEndpoint>();
    c.WithModule<UpdateProductEndpoint>();
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

var app = builder.Build();

app.MapCarter();

app.Run();
