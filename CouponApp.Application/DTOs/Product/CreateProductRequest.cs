namespace CouponApp.Application.DTOs.Product
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
