using Microsoft.AspNetCore.Identity;
using CouponApp.Application;
using CouponApp.Application.DTOs.Auth;
using CouponApp.Persistence.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using CouponApp.Application.Interfaces.Sercives.Auth;

namespace CouponApp.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<EmailConfirmationResult>> RegisterAsync(RegisterUserRequest request)
        {
            if (request == null)
                return Result<EmailConfirmationResult>.Failure("Invalid request");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = false,
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors
                    .Select(e => e.Description);

                return Result<EmailConfirmationResult>.Failure(errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // encode token to url
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var result = new EmailConfirmationResult
            {
                UserId = user.Id,
                Token = encodedToken,
                Email = user.Email
            };

            return Result<EmailConfirmationResult>.Success(result);
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result.Failure("User not found");
            }

            // decode token
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                return Result.Failure(result.Errors.Select(e => e.Description));
            }

            return Result.Success();
        }

    }
}
