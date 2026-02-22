using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CouponApp.Domain.Entity;

namespace CouponApp.Persistence.Configurations
{
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.ToTable("Merchants");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.BusinessName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasMany(m => m.Offers)
               .WithOne(o => o.Merchant)
               .HasForeignKey(o => o.MerchantId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => m.UserId)
                .IsUnique();
        }
    }

}
