using BuildingBlocks.Behaviors;
using Catalog.API.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(configurator: c =>
{
    c.WithProductEndpoints();
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

var app = builder.Build();

app.MapCarter();

app.Run();
