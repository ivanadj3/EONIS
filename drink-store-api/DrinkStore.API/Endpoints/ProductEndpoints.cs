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
            return Results.Ok(new { Products = "abc" });
        }
    }
}
