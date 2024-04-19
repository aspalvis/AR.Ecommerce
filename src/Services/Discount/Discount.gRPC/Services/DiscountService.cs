using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid argument"));

            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();

            CouponModel couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            await dbContext
                .Coupons
                .Where(x => x.ProductName == request.ProductName)
                .ExecuteDeleteAsync();

            return new DeleteDiscountResponse { Success = true };
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext
                .Coupons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            coupon ??= new Coupon { ProductName = "No discount", Amount = 0, Description = "Coupon do not have any discounts" };

            logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}, Description : {description}", coupon.ProductName, coupon.Amount, coupon.Description);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = request.Coupon.Adapt<Coupon>()
                ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid argument"));

            Coupon couponFromDb = await dbContext.Coupons.FirstOrDefaultAsync(x => x.Id == coupon.Id)
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Discount do not exists"));

            couponFromDb.Amount = coupon.Amount;
            couponFromDb.Description = coupon.Description;
            couponFromDb.ProductName = coupon.ProductName;

            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is updated with ProductName : {productName}, Amount : {amount}, Description : {description}", coupon.ProductName, coupon.Amount, coupon.Description);

            return couponFromDb.Adapt<CouponModel>();
        }
    }
}
