using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CouponApp.Persistence.Identity;

namespace CouponApp.Persistence.Configurations
{
    public class ApplicationUserConfiguration
    : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(x => x.DomainUserId)
                   .IsUnique()
                   .HasFilter("[DomainUserId] IS NOT NULL");
        }
    }

}
