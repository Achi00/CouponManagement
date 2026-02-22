using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
