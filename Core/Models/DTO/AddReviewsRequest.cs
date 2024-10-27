using System.ComponentModel.DataAnnotations;

namespace Core.Models.DTO
{
    public class AddReviewsRequest
    {
        public Guid ReviewsID { get; set; }
        public int ProductsID { get; set; }
        public int UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public required string ReviewTitle { get; set; }
        [Required]
        [MaxLength(1000)]
        public required string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserCreated { get; set; }
    }
}
