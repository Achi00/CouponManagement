
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CouponApp.Domain.Entity;

namespace CouponApp.Persistence.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.StockQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
