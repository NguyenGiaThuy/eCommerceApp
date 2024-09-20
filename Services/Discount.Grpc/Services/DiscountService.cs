namespace eCommerceApp.Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger<DiscountService> logger)
    : Discount.DiscountBase
{
    public override async Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductId == new Guid(request.ProductId));
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount for ProductId={request.ProductId} is not found."));

        logger.LogInformation($"Discount is retrieved for ProductId={coupon.ProductId}");

        var couponModel = coupon.Adapt<CouponModel>();
        return new GetDiscountResponse { Coupon = couponModel };
    }

    public override async Task<CreateDiscountResponse> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is created for ProductId={coupon.ProductId}");

        return new CreateDiscountResponse { ProductId = coupon.ProductId.ToString() };
    }

    public override async Task<UpdateDiscountResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is updated for ProductId={coupon.ProductId}");

        return new UpdateDiscountResponse { ProductId = coupon.ProductId.ToString() };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductId == new Guid(request.ProductId));
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount for ProductId={request.ProductId} not found"));

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is removed for ProductId={coupon.ProductId}");

        return new DeleteDiscountResponse { IsSuccess = true };
    }
}