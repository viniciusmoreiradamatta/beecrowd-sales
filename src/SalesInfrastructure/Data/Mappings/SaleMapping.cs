using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDomain.Entities.Sale;

namespace SalesInfrastructure.Data.Mappings;

internal class SaleMapping : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleNumber)
               .ValueGeneratedOnAdd();

        builder.Property(s => s.SaleDate)
               .IsRequired();

        builder.Property(s => s.CustomerId)
                   .IsRequired();

        builder.Property(s => s.BranchId)
               .IsRequired();

        builder.Property(s => s.TotalAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(s => s.Cancelled)
               .IsRequired();

        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey(c => c.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}