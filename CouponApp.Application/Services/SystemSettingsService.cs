using CouponApp.Application.DTOs.Settings;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Enums;
using Mapster;

namespace CouponApp.Application.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly ISystemSettingsRepository _systemSettingsRepository;
        private readonly IAuthorizationService _authorization;
        private readonly IUnitOfWork _unitOfWork;

        public SystemSettingsService(ISystemSettingsRepository systemSettingsRepository, IAuthorizationService authorization, IUnitOfWork unitOfWork)
        {
            _systemSettingsRepository = systemSettingsRepository;
            _authorization = authorization;
            _unitOfWork = unitOfWork;
        }
        public async Task<SystemSettingsResponse> GetAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            var settings = await _systemSettingsRepository.GetAsync(cancellationToken);

            return settings.Adapt<SystemSettingsResponse>();
        }

        public async Task UpdateAsync(UpdateSystemSettingsRequest request, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            var settings = await _systemSettingsRepository.GetAsync(cancellationToken);

            request.Adapt(settings);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
