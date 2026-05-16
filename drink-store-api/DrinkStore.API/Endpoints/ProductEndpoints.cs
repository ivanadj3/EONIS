using DrinkStore.API.Db;
using DrinkStore.API.Dto;
using DrinkStore.API.Models;
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
            group.MapPost("/", CreateProductAsync);
            group.MapDelete("/{id}", DeleteProductAsync);
            group.MapPut("/{id}", UpdateProductAsync);
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

        private static async Task<IResult> CreateProductAsync(DrinkStoreDbContext dbContext, ReqCreateProduct dto)
        {
            var exists = await dbContext.Products.Where(x => x.Name == dto.Name).AnyAsync();
            if (exists) return Results.BadRequest();

            var product = new Product
            {
                Image = dto.Image,
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
            };

            await dbContext.Products.AddAsync(product);

            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> DeleteProductAsync(DrinkStoreDbContext dbContext, int id)
        {
            await dbContext.Products.Where(x => x.Id == id).ExecuteDeleteAsync();

            return Results.Ok();
        }

        private static async Task<IResult> UpdateProductAsync(DrinkStoreDbContext dbContext, int id, ReqCreateProduct dto)
        {
            var product = await dbContext.Products.Where(x => x.Name == dto.Name).FirstOrDefaultAsync();
            if (product == null) return Results.BadRequest();

            product.Image = dto.Image;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;

            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }
    }
}

