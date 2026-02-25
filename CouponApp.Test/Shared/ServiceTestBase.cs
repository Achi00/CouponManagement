using CouponApp.Application.Interfaces.Sercives.Auth;
using Moq;

namespace CouponApp.Test.Shared
{
    public abstract class ServiceTestBase
    {
        protected readonly Mock<ICurrentUserService> CurrentUser = new();
        protected readonly Mock<IAuthorizationService> Authorization = new();

        protected Guid UserId = Guid.NewGuid();

        protected ServiceTestBase()
        {
            CurrentUser
                .Setup(x => x.UserId)
                .Returns(UserId);

            CurrentUser
                .Setup(x => x.IsAuthenticated)
                .Returns(true);
        }
    }
}
