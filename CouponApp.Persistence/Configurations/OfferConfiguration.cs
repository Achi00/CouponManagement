using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CouponApp.Domain.Entity;

namespace CouponApp.Persistence.Configurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.ToTable("Offers");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.Description)
                .HasMaxLength(2000);

            builder.Property(o => o.OriginalPrice)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.DiscountedPrice)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.RejectionReason)
                .HasMaxLength(500);

            builder.HasOne(o => o.Merchant)
            .WithMany(m => m.Offers)
            .HasForeignKey(o => o.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Category)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.CategoryId);
            builder.HasIndex(o => o.MerchantId);
        }
    }
}
