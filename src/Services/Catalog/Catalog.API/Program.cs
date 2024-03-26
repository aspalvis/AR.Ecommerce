WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IWebHostEnvironment environment = builder.Environment;
ConfigurationManager configuration = builder.Configuration;

services.AddCarter(configurator: c =>
{
    c.WithProductEndpoints();
});

services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

services.AddValidatorsFromAssembly(typeof(Program).Assembly);

services.AddMarten(options =>
{
    options.Connection(configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

if (environment.IsDevelopment())
{
    services.InitializeMartenWith<CatalogInitialData>();
}

services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
