namespace CouponApp.Web.Models.Shared
{
    public class OperationResultViewModel
    {
        public bool Success { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public string? RedirectAction { get; set; }
        public string? RedirectController { get; set; }
        public string? RedirectText { get; set; }
    }

}
