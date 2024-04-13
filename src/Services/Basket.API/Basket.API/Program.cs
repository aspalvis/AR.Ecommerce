var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddCarter();
services.AddMediatR(c =>
{
    c.RegisterServicesFromAssembly(typeof(Program).Assembly);
    c.AddOpenBehavior(typeof(ValidationBehavior<,>));
    c.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var app = builder.Build();

app.MapCarter();

app.Run();
