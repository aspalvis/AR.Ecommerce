using Marten;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddCarter();
services.AddMediatR(c =>
{
    c.RegisterServicesFromAssembly(typeof(Program).Assembly);
    c.AddOpenBehavior(typeof(ValidationBehavior<,>));
    c.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

services.AddMarten(c =>
{
    c.Connection(builder.Configuration.GetConnectionString("Database")!);
    c.Schema.For<ShoppingCart>().Identity(x => x.Username);
}).UseLightweightSessions();

var app = builder.Build();

app.MapCarter();

app.Run();
