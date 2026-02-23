using CouponApp.Application.DTOs.Categories;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using Mapster;
using System.Net;

namespace CouponApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IAuthorizationService _authorization;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IOfferRepository offerRepository, IAuthorizationService authorization, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _offerRepository = offerRepository;
            _authorization = authorization;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            var category = request.Adapt<Category>();

            var existingCategory = await _categoryRepository.GetByName(category.Name, cancellationToken);

            if (existingCategory == null)
            {
                throw new BusinessException("Category already exists");
            }
            _categoryRepository.Add(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            var hasOffers = await _offerRepository.ExistsByCategoryIdAsync(categoryId, cancellationToken);

            if (hasOffers)
            {
                throw new BusinessException("Can not delete category because offers use it");
            }

            _categoryRepository.Delete(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();

            return await _categoryRepository.GetAllAsync(cancellationToken);
        }

        public async Task UpdateAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            var category = await _categoryRepository.GetForUpdateAsync(categoryId, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            request.Adapt(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
