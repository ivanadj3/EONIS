using DrinkStore.API.Db;
using DrinkStore.API.Dto;
using DrinkStore.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DrinkStore.API.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/orders");

            group.MapPost("/", MakeOrderAsync).RequireAuthorization();
        }

        private static async Task<IResult> MakeOrderAsync(DrinkStoreDbContext dbContext, ReqMakeOrder dto, IConfiguration config, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));

            var order = new Order()
            {
                CreatedAt = DateTime.UtcNow,
                User = user,
                Address = user.Address
            };

            var createdOrder = await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var dbItems = new List<OrderItem>();
            foreach (var item in dto.Items)
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                if (product == null) throw new Exception("Product not found");

                var dbItem = new OrderItem()
                {
                    Order = order,
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    TotalPrice = product.Price * item.Quantity
                };

                dbItems.Add(dbItem);
            }

            await dbContext.AddRangeAsync(dbItems);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                OrderId  = order.Id
            });
        }
    }
}
