using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Models
{
    public class DisplayProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }

        // Computed/formatted properties for UI
        public string PriceFormatted => $"${Price:N2}";

        [Display(Name = "Stock")]
        public int StockQuantity { get; set; }

        // UI-specific computed properties
        public string StockStatus => StockQuantity switch
        {
            0 => "Out of Stock",
            < 15 => "Low Stock",
            _ => "In Stock"
        };

        public bool IsLowStock => StockQuantity < 10;

        public string StockStatusCssClass => StockQuantity switch
        {
            0 => "badge bg-danger",
            < 10 => "badge bg-warning",
            _ => "badge bg-success"
        };
    }
}
