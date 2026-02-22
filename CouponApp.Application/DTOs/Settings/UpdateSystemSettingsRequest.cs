namespace CouponApp.Application.DTOs.Settings
{
    public class UpdateSystemSettingsRequest
    {
        public int ReservationDurationMinutes { get; set; }

        public int MerchantEditPeriodHours { get; set; }
    }
}
