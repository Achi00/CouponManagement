using CouponApp.Application.DTOs.Auth;

namespace CouponApp.Application.Interfaces.Sercives.Auth
{
    public interface IAuthService
    {
        Task<Result<EmailConfirmationResult>> RegisterAsync(RegisterUserRequest request);
        Task<Result> ConfirmEmailAsync(string userId, string token);
    }
}
