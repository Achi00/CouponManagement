using CouponApp.Domain.Entity;
using CouponApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CouponApp.Persistence.Seeds
{
    public static class SeedCategories
    {
        public static async Task SeedCategoriesAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<DiscountManagementContext>();

            var cancellationToken = CancellationToken.None;

            var exists = await context.Categories
                .AnyAsync(cancellationToken);

            if (exists)
            {
                return;
            }

            var categories = new List<Category>
            {
                new() { Name = "Food & Drink" },
                new() { Name = "Beauty & Wellness" },
                new() { Name = "Travel & Hotels" },
                new() { Name = "Sports & Fitness" },
                new() { Name = "Electronics" },
                new() { Name = "Fashion & Clothing" },
                new() { Name = "Entertainment" },
                new() { Name = "Home & Garden" },
            };

            context.Categories.AddRange(categories);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
