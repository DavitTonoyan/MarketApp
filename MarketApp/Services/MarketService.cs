using MarketApp.DataTransferObjects;
using MarketApp.Models;

namespace MarketApp.Services
{
    public class MarketService : IMarketService
    {
        private readonly MarketContext _context;

        public MarketService(MarketContext context)
        {
            _context = context;
        }

        public async Task AddProduct(ProductDto request)
        {
            Product market = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Amount = request.Amount
            };

            _context.Market.Add(market);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Market.FindAsync(id);

            if (product is null)
                throw new ArgumentException(" Incorrect id entered");
        }

        public IAsyncEnumerable<Product> GetAllProducts()
        {
            var products = _context.Market.ToAsyncEnumerable();
            return products;
        }

        public async Task UpdateProduct(int id, ProductDto request)
        {
            var product = await _context.Market.FindAsync(id);

            if (product is null)
                throw new ArgumentException(" Incorrect id of product ");

            product.Name = request.Name;
            product.Price = request.Price;
            product.Amount = request.Amount;

            await _context.SaveChangesAsync();  
        }
    }
}
