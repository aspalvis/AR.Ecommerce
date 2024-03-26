using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IWebHostEnvironment environment = builder.Environment;
ConfigurationManager configuration = builder.Configuration;

string connectionString = configuration.GetConnectionString("CatalogDb")!;

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
    options.Connection(connectionString);
}).UseLightweightSessions();

if (environment.IsDevelopment())
{
    services.InitializeMartenWith<CatalogInitialData>();
}

services.AddExceptionHandler<CustomExceptionHandler>();

services.AddHealthChecks()
    .AddNpgSql(connectionString);

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
