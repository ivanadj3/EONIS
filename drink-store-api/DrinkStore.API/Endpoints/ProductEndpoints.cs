using DrinkStore.API.Dto;

namespace DrinkStore.API.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/products");

            group.MapGet("/", GetProducts);
        }

        private static IResult GetProducts()
        {
            return Results.Ok(new List<ProductDto>() {
                new ProductDto {
                    Image = "",
                    Name = "Product 1",
                    Stock = 2,
                    Price = 300
                },
                new ProductDto {
                    Image = "",
                    Name = "Dang",
                    Stock = 56,
                    Price = 1000
                }
            });
        }
    }
}
