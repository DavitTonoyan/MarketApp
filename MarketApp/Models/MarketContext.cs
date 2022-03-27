using Microsoft.EntityFrameworkCore;

namespace MarketApp.Models
{
    public class MarketContext :DbContext
    {
        public DbSet<Product> Market { get; set; }
        public DbSet<User> Users { get; set; }

        public MarketContext(DbContextOptions<MarketContext> options)
            : base(options)
        {
        }
    }
}
