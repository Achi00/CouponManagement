using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Areas.Admin.ViewModels
{
    public class SystemSettingsViewModel
    {
        [Range(1, 120)]
        public int ReservationDurationMinutes { get; set; }

        [Range(1, 72)]
        public int MerchantEditPeriodHours { get; set; }
    }
}
