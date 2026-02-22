namespace CouponApp.Application.Interfaces
{
    public interface ICouponCodeGenerator
    {
        string Generate(int length = 10);
    }
}
