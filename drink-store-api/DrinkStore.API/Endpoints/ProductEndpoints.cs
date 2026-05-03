using DrinkStore.API.Db;
using DrinkStore.API.Dto;
using Microsoft.EntityFrameworkCore;

namespace DrinkStore.API.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/products");

            group.MapGet("/", GetProductsAsync);
            group.MapGet("/{id}", GetProductByIdAsync);
        }

        private static async Task<IResult> GetProductsAsync(DrinkStoreDbContext dbContext, string? search = null, string sortBy = "Name", string sort = "asc", int pageSize = 10, int page = 1)
        {
            var query = dbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (sort == "asc")
                query = query.OrderBy(p => p.Price);
            else if (sort == "desc")
                query = query.OrderByDescending(p => p.Price);

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new ProductDto
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name,
                Stock = x.Stock,
                Price = x.Price
            });

            return Results.Ok(new PaginatedDto<IEnumerable<ProductDto>>
            {
                Data = dtos,
                Page = page,
                PageSize = pageSize,
                Total = total
            });
        }

        private static async Task<IResult> GetProductByIdAsync(DrinkStoreDbContext dbContext, int id)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return Results.NotFound();

            return Results.Ok(new ProductDto
            {
                Id = product.Id,
                Image = product.Image,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price
            });
        }
    }
}
