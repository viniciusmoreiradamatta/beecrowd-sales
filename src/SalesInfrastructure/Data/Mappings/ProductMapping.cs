using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDomain.Entities.Product;

namespace SalesInfrastructure.Data.Mappings;

internal class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.Description)
               .HasMaxLength(80)
               .IsRequired();

        builder.Property(p => p.Category)
               .HasMaxLength(80)
               .IsRequired();

        builder.Property(p => p.Image)
               .HasMaxLength(255)
               .IsRequired();
    }
}