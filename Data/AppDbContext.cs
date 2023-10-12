using Microsoft.EntityFrameworkCore;
using food_delivery.Data.Models;

namespace food_delivery.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AddressElement> AddressElements { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishInCart> DishesInCart { get; set; }

        public DbSet<Hirerarhy> Hirerarhy { get; set; }

        public DbSet<House> Houses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}