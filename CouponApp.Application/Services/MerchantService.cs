using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Domain.Entity;
using Mapster;

namespace CouponApp.Application.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MerchantService(IMerchantRepository merchantRepository, IUnitOfWork unitOfWork)
        {
            _merchantRepository = merchantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MerchantProfileResponse> GetByIdAsync(Guid merchantId, CancellationToken cancellationToken)
        {
            var merchant = await _merchantRepository.GetByIdAsync(merchantId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException("Merchant was not found");
            }

            return merchant.Adapt<MerchantProfileResponse>();
        }

        public async Task<MerchantProfileResponse> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException("Merchant was not found");
            }

            return merchant.Adapt<MerchantProfileResponse>();
        }

        public Task<MerchantDashboardResponse> GetDashboardAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<MerchantSaleResponse>> GetSalesHistoryAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid userId, UpdateProfileMerchantRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
