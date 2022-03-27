using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(90)")]
        public byte[] PasswordHash { get; set; }

        [Required]
        [Column(TypeName = "varchar(190)")]
        public byte[] PasswordSalt { get; set; }

        [Range(0,double.MaxValue)]
        public double Money { get; set; }
    }
}
