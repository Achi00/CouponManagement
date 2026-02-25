using CouponApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponApp.Persistence.Configurations
{
    public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
    {
        public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                .HasMaxLength(500);

            builder.Property(x => x.Path)
                .HasMaxLength(500);

            builder.Property(x => x.Method)
                .HasMaxLength(10);
        }
    }
}
