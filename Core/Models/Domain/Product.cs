using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core.Models.Domain
{
    public class Product : BaseEntity
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }

        [Required]
        [Column(TypeName = "Decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public required string Type { get; set; }

        [Required]
        public required string PictureUrl { get; set; }

    }
}
