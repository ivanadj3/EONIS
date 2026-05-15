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
            group.MapGet("/", FetchOrdersAsync).RequireAuthorization();
            group.MapGet("/{id}", FetchOrderByIdAsync).RequireAuthorization();
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
                OrderId = order.Id
            });
        }

        private static async Task<IResult> FetchOrdersAsync(DrinkStoreDbContext dbContext, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue(
               ClaimTypes.NameIdentifier
           );

            var orders = await dbContext.Orders.Include(x => x.Items).Where(x => x.User.Id == int.Parse(userId)).OrderByDescending(x => x.Id).Select(x => new ResOrder
            {
                Id = x.Id,
                Paid = x.Paid,
                CreatedAt = x.CreatedAt,
                TotalAmount = x.Items.Sum(x => x.TotalPrice),
                Items = x.Items.Select(x => new ResOrderItem
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice
                })
            }).ToListAsync();

            return Results.Ok(new
            {
                Data = orders
            });
        }

        private static async Task<IResult> FetchOrderByIdAsync(DrinkStoreDbContext dbContext, ClaimsPrincipal claimsPrincipal, int id)
        {
            var userId = claimsPrincipal.FindFirstValue(
               ClaimTypes.NameIdentifier
           );

            var order = await dbContext.Orders.Include(x => x.Items).Include(x => x.User).Where(x => x.Id == id && x.User.Id == int.Parse(userId)).Select(x => new ResOrder
            {
                Id = x.Id,
                Paid = x.Paid,
                CreatedAt = x.CreatedAt,
                TotalAmount = x.Items.Sum(x => x.TotalPrice),
                Items = x.Items.Select(x => new ResOrderItem
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice
                }),
                Address = x.Address,
                User = x.User.Name + " " + x.User.Surname,
                Phone = x.User.Phone
            }).FirstOrDefaultAsync();

            if (order == null) return Results.BadRequest();

            return Results.Ok(new
            {
                Data = order
            });
        }
    }
}
