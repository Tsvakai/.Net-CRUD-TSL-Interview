using Microsoft.EntityFrameworkCore;
using RestApiExample.Models;

namespace RestApiExample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        protected override void OnModelCreating(ModelBuilder modelBuilder){
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Product>(entity =>
    {
        entity.OwnsOne(p => p.Pricing);
        entity.OwnsOne(p => p.Inventory);
        entity.OwnsOne(p => p.ProductMetadata);
    });
     }
    }

}
