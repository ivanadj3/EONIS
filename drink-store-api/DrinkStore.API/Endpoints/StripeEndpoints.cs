using DrinkStore.API.Db;
using DrinkStore.API.Dto;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Security.Claims;

namespace DrinkStore.API.Endpoints
{
    public static class StripeEndpoints
    {
        public static void MapAStripeEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/stripe");

            group.MapPost("/session", CreateStripeSessionAsync).RequireAuthorization();
        }

        private static async Task<IResult> CreateStripeSessionAsync(DrinkStoreDbContext dbContext, ReqCreateStripeSession dto, HttpContext context, IConfiguration config, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue(
               ClaimTypes.NameIdentifier
            );

            var order = await dbContext.Orders.Include(x => x.Items).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == dto.OrderId);
            if (order == null) throw new Exception("Order not found");

            if (order.User.Id != int.Parse(userId)) throw new Exception("No order found");

            var frontendUrl = context.Request.Headers.Origin.ToString();
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{frontendUrl}/order/{dto.OrderId}",
                CancelUrl = $"{frontendUrl}/",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = order.Items.Select(x => new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(x.TotalPrice / x.Quantity) * 100,
                        Currency = "RSD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = x.ProductName,
                            Description = x.ProductName,
                        },
                    },
                    Quantity = x.Quantity,
                }).ToList(),
                Mode = "payment",
                Metadata = new Dictionary<string, string>
                {
                    {"orderId", dto.OrderId.ToString() }
                },
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Results.Ok(new
            {
                SessionUrl = session.Url
            });
        }
    }
}
