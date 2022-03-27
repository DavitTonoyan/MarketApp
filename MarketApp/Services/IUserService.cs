using MarketApp.DataTransferObjects;

namespace MarketApp.Services
{
    public interface IUserService
    {
        Task Register(UserDto request, double Money);
        Task<string> Login(UserDto request);
        string LoginToAdminAccount(UserDto request);
        Task BuyProduct(int productId, int amount);
    }
}
