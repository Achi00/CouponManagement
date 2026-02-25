using CouponApp.Application.DTOs.Categories;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Services;
using CouponApp.Domain.Entity;
using CouponApp.Test.Shared;
using FluentAssertions;
using Moq;

namespace CouponApp.Test.Services
{
    public class CategoryServiceTests : ServiceTestBase
    {
        private readonly Mock<ICategoryRepository> _categories = new();
        private readonly Mock<IOfferRepository> _offers = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _service = new CategoryService(
                _categories.Object,
                _offers.Object,
                Authorization.Object,
                _unitOfWork.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Category_When_Name_Is_Unique()
        {
            _categories.Setup(x => x.GetByName("Electronics", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await _service.CreateAsync(new CreateCategoryRequest { Name = "Electronics" }, default);

            _categories.Verify(x => x.Add(It.Is<Category>(c => c.Name == "Electronics")), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Category_Already_Exists()
        {
            var existing = TestData.CreateCategory("Electronics");

            _categories.Setup(x => x.GetByName("Electronics", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            Func<Task> act = () => _service.CreateAsync(new CreateCategoryRequest { Name = "Electronics" }, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Category already exists");
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Category_When_No_Offers_Use_It()
        {
            var category = TestData.CreateCategory();

            _categories.Setup(x => x.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            _offers.Setup(x => x.ExistsByCategoryIdAsync(category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await _service.DeleteAsync(category.Id, default);

            _categories.Verify(x => x.Delete(category), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_When_Category_Has_Offers()
        {
            var category = TestData.CreateCategory();

            _categories.Setup(x => x.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            _offers.Setup(x => x.ExistsByCategoryIdAsync(category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> act = () => _service.DeleteAsync(category.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Can not delete category because offers use it");
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_NotFoundException_When_Category_Not_Found()
        {
            _categories.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            Func<Task> act = () => _service.DeleteAsync(Guid.NewGuid(), default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category not found");
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_NotFoundException_When_Category_Not_Found()
        {
            _categories.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            Func<Task> act = () => _service.UpdateAsync(Guid.NewGuid(), new UpdateCategoryRequest(), default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category not found");
        }

        [Fact]
        public async Task UpdateAsync_Should_Save_When_Category_Found()
        {
            var category = TestData.CreateCategory();

            _categories.Setup(x => x.GetForUpdateAsync(category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            await _service.UpdateAsync(category.Id, new UpdateCategoryRequest { Name = "Updated" }, default);

            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
