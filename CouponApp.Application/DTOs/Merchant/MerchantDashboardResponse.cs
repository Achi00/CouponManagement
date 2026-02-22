namespace CouponApp.Application.DTOs.Merchant
{
    public class MerchantDashboardResponse
    {
        public int ActiveOffers { get; set; }

        public int PendingOffers { get; set; }

        public int ExpiredOffers { get; set; }

        public int TotalCouponsSold { get; set; }
    }
}
