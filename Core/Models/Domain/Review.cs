using System.ComponentModel.DataAnnotations;

namespace Core.Models.Domain
{
    public class Review : BaseEntity
    {
        public int ProductsID { get; set; } 
        public int UserId { get; set; }
        public required string ReviewTitle { get; set; }
        public required string Comment { get; set; }
        public int Rating { get; set; }

        // Navigation property
        public Product? Products { get; set; }
    }
}
