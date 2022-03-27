using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketApp.Models
{
    [Table("Market")]
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
