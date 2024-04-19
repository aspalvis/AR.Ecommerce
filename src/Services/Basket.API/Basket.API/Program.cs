using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;
using Discount.gRPC;
using HealthChecks.UI.Client;
using Marten;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

services.AddCarter();
services.AddMediatR(c =>
{
    c.RegisterServicesFromAssembly(typeof(Program).Assembly);
    c.AddOpenBehavior(typeof(ValidationBehavior<,>));
    c.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

services.AddMarten(c =>
{
    c.Connection(databaseConnectionString);
    c.Schema.For<ShoppingCart>().Identity(x => x.Username);
}).UseLightweightSessions();

services.AddStackExchangeRedisCache(c =>
{
    c.Configuration = redisConnectionString;
});

services.AddScoped<IBasketRepository, BasketRepository>();
services.Decorate<IBasketRepository, CachedBasketRepository>();

//Manual decorating
//services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<IBasketRepository>();
//    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
//});

services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(c =>
{
    c.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});

services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString);

services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
