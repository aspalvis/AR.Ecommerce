using Discount.gRPC.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; } = default!;

        public DiscountContext(DbContextOptions<DiscountContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon() { Id = 1, ProductName = "IPhone X", Description = "Iphone discount", Amount = 80 },
                new Coupon() { Id = 2, ProductName = "Samsung 10", Description = "Samsung discount", Amount = 40 }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
