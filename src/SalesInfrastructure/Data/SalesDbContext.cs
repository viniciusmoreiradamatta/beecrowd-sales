using Microsoft.EntityFrameworkCore;
using SalesDomain.Entities.Product;
using SalesDomain.Entities.Sale;
using System.Reflection;

namespace SalesInfrastructure.Data;

public class SalesDbContext(DbContextOptions<SalesDbContext> op) : DbContext(op)
{
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var types in Assembly.GetAssembly(typeof(Sale))?.GetTypes()
                                      .Where(c => c.IsSubclassOf(typeof(SalesDomain.Entities.Entity))) ?? throw new InvalidOperationException())
        {
            modelBuilder.Entity(types).Ignore("Valid");
            modelBuilder.Entity(types).Ignore("Notifications");
            modelBuilder.Entity(types).Ignore("ValidationResult");
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
    }
}