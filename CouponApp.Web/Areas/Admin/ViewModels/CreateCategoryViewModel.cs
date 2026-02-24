using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Areas.Admin.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
