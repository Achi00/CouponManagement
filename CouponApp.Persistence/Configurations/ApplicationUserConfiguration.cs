using CouponApp.Domain.Enums;
using CouponApp.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponApp.Persistence.Configurations
{
    public class ApplicationUserConfiguration
    : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.Role)
                .IsRequired()
                .HasDefaultValue(UserRole.Customer)
                .HasConversion<int>();

            builder.Property(u => u.IsBlocked)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }

}
