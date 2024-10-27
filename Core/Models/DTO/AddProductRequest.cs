using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.DTO
{
    public class AddProductRequest
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
        public required string UserCreated { get; set; }
        public ImageUploadRequest? Images { get; set; }
    }
}
