using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CouponApp.Domain.Entity;
using CouponApp.Persistence.Identity;

namespace CouponApp.Persistence.Contexts
{
    public class DiscountManagementContext : IdentityDbContext<ApplicationUser>
    {
        public DiscountManagementContext(DbContextOptions<DiscountManagementContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountManagementContext).Assembly);
        }
    }
}
