using AutoMapper;
using MarketApp.DataTransferObjects;
using MarketApp.Models;

namespace MarketApp.Services
{
    public class MarketService : IMarketService
    {
        private readonly MarketContext _context;
        private readonly IMapper _mapper;

        public MarketService(MarketContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddProduct(ProductDto request)
        {
            Product product = _mapper.Map<Product>(request);

            _context.Market.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Market.FindAsync(id);

            if (product is null)
                throw new ArgumentException(" Incorrect id entered");

            _context.Market.Remove(product);
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

            product = _mapper.Map<Product>(request);
            product.Id = id;

            await _context.SaveChangesAsync();  
        }
    }
}
