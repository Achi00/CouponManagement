namespace CouponApp.Application.DTOs.Auth
{
    public class EmailConfirmationResult
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }

}
