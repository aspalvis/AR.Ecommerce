using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Data
{
    public static class Extensions
    {
        public async static Task UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
