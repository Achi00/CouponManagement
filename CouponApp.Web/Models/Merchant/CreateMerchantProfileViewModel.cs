using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Models.Merchant
{
    public class CreateMerchantProfileViewModel
    {
        [Required]
        [StringLength(120)]
        public string BusinessName { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
    }
}
