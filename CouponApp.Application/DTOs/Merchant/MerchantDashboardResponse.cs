namespace CouponApp.Application.DTOs.Merchant
{
    public class MerchantDashboardResponse
    {
        public int TotalOffers { get; set; }
        public int TotalCouponsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveOffers { get; set; }
        public List<MerchantSaleResponse> RecentSales { get; set; }
    }
}
