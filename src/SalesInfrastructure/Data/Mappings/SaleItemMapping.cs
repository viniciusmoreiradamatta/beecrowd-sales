using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesDomain.Entities.Sale;

namespace SalesInfrastructure.Data.Mappings;

internal class SaleItemMapping : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);

        builder.Property(si => si.ProductId)
               .IsRequired();

        builder.Property(si => si.SaleId)
               .IsRequired();

        builder.Property(si => si.Quantity)
               .IsRequired();

        builder.Property(si => si.UnitPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(si => si.ValueMonetaryTaxApplied)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(si => si.Total)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(si => si.IsCancelled)
               .IsRequired();
    }
}