using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CouponApp.Domain.Entity;

namespace CouponApp.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasMany(c => c.Offers)
                   .WithOne(o => o.Category)
                   .HasForeignKey(o => o.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
