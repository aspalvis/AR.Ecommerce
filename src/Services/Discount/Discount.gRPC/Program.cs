using Discount.gRPC.Data;
using Discount.gRPC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(o =>
{
    o.UseSqlite(builder.Configuration.GetConnectionString("Database")!);
});

var app = builder.Build();

await app.UseMigration();

app.MapGrpcService<DiscountService>();
app.Run();
