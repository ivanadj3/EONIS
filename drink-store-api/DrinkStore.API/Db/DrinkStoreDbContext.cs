using DrinkStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkStore.API.Db
{
    public class DrinkStoreDbContext : DbContext
    {
        public DrinkStoreDbContext(DbContextOptions<DrinkStoreDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    }
}
