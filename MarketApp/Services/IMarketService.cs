using MarketApp.DataTransferObjects;
using MarketApp.Models;

namespace MarketApp.Services
{
    public interface IMarketService
    {
        IAsyncEnumerable<Product> GetAllProducts();

        Task AddProduct(ProductDto request);

        Task UpdateProduct(int id, ProductDto request);

        Task DeleteProduct(int id);
    }
}
