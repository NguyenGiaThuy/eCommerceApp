namespace eCommerceApp.Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<ApplyDiscountResponse> ApplyDiscount(ApplyDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.Id == new Guid(request.Id));
        if (coupon == null) return new ApplyDiscountResponse
        {
            Coupon = new CouponModel
            {
                Id = "",
                ProductId = "",
                ProductName = "No discount",
                Description = "No discount",
                Amount = 0
            }
        };

        logger.LogInformation($"Discount is applied for Id={request.Id}");

        return new ApplyDiscountResponse { Coupon = coupon.Adapt<CouponModel>() };
    }

    public override async Task<GetDiscountsResponse> GetDiscounts(GetDiscountsRequest request, ServerCallContext context)
    {
        var coupons = await dbContext.Coupons
            .Where(x => x.ProductId == new Guid(request.ProductId)).ToListAsync();

        logger.LogInformation($"All discounts are retrieved for ProductId={request.ProductId}");

        var response = new GetDiscountsResponse();
        foreach (var coupon in coupons)
            response.Coupons.Add(coupon.Adapt<CouponModel>());

        return response;
    }

    public override async Task<CreateDiscountResponse> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        request.Coupon.Id = Guid.NewGuid().ToString();
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is created for Id={request.Coupon.Id}");

        return new CreateDiscountResponse { Id = request.Coupon.Id };
    }

    public override async Task<UpdateDiscountResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is updated for Id={request.Coupon.Id}");

        return new UpdateDiscountResponse { Id = request.Coupon.Id };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.Id == new Guid(request.Id));
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount for Id={request.Id} not found"));

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is removed for Id={request.Id}");

        return new DeleteDiscountResponse { IsSuccess = true };
    }
}