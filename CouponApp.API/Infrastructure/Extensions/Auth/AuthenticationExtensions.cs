using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Application.Services.Auth;
using CouponApp.Infrastructure.Auth;
using CouponApp.Persistence.Identity;
using CouponApp.API.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace CouponApp.API.Infrastructure.Extensions.Auth
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAuthService, AuthService>();

            var jwt = config.GetSection("Jwt");

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt["Key"]!)
                        )
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(
                                JsonSerializer.Serialize(new
                                {
                                    error = "Unauthorized",
                                    message = "Invalid or missing token"
                                }));
                        },
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(
                                JsonSerializer.Serialize(new
                                {
                                    error = "Forbidden",
                                    message = "You do not have permission"
                                }));
                        }
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
