using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CouponApp.Application.DTOs.Auth;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Persistence.Identity;
using CouponApp.Web.Attributes;
using CouponApp.Web.Models.Account;
using CouponApp.Web.Models.Shared;

namespace CouponApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;

        public AccountController(
            SignInManager<ApplicationUser> signInManager, 
            IEmailService emailService, 
            UserManager<ApplicationUser> userManager,
            IAuthService authService)
        {
            _signInManager = signInManager;
            _emailService = emailService;
            _userManager = userManager;
            _authService = authService;
        }

        // login
        [HttpGet]
        [RedirectIfAuthenticated]
        public IActionResult Login(string? email)
        {
            var model = new LoginViewModel
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Account does not exist.");
                return View(model);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError("", "Incorrect password.");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email before logging in.");
                return View(model);
            }

            if (user.IsBlocked)
            {
                ModelState.AddModelError("", "Your account has been blocked.");
                return View(model);
            }

            await _signInManager.SignInAsync(user, model.RememberMe);

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // register
        [HttpGet]
        [RedirectIfAuthenticated]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = model.Adapt<RegisterUserRequest>();

            var result = await _authService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

                return View(model);
            }

            var data = result.Value;

            // Build confirmation link in controller
            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = data.UserId, token = data.Token },
                Request.Scheme);

            // send email
            await _emailService.SendAsync(
                data.Email,
                "Confirm your email",
                $"Click here to confirm: {confirmationLink}");

            return RedirectToAction("Login", new { email = model.Email });
        }

        // email confirmation
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authService.ConfirmEmailAsync(userId, token);

            if (!result.Succeeded)
            {
                return View("OperationResult", new OperationResultViewModel
                {
                    Success = false,
                    Title = "Email confirmation failed",
                    Message = "The link is invalid or expired.",
                    RedirectAction = "Login",
                    RedirectController = "Account",
                    RedirectText = "Back to login"
                });
            }

            return View("OperationResult", new OperationResultViewModel
            {
                Success = true,
                Title = "Email confirmed",
                Message = "Your email has been successfully confirmed.",
                RedirectAction = "Login",
                RedirectController = "Account",
                RedirectText = "Go to login"
            });
        }


        // pasword reset
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !user.EmailConfirmed)
            {
                return View("OperationResult", new OperationResultViewModel
                {
                    Success = true,
                    Title = "Check your email",
                    Message = "If an account with that email exists, a reset link has been sent.",
                    RedirectAction = "Login",
                    RedirectController = "Account",
                    RedirectText = "Back to login"
                });

            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { email = user.Email, token = token },
                Request.Scheme);

            await _emailService.SendAsync(
                user.Email,
                "Reset your password",
                $"Click here to reset your password: {resetLink}");

            return View("OperationResult", new OperationResultViewModel
            {
                Success = true,
                Title = "Password reset successful",
                Message = "Your password has been updated.",
                RedirectAction = "Login",
                RedirectController = "Account",
                RedirectText = "Go to login"
            });

        }

        // reset password
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
                return BadRequest();

            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return View("OperationResult", new OperationResultViewModel
                {
                    Success = true,
                    Title = "Password reset successful",
                    Message = "Your password has been updated.",
                    RedirectAction = "Login",
                    RedirectController = "Account",
                    RedirectText = "Go to login"
                });

            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return View("ResetPasswordConfirmation");
        }
    }
}
