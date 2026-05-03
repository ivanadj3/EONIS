using DrinkStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkStore.API.Db
{
    public class DrinkStoreDbContext : DbContext
    {
        public DrinkStoreDbContext(DbContextOptions<DrinkStoreDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }
}
