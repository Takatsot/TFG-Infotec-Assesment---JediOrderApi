namespace Core.Models.DTO
{
    public class ReviewResponse
    {
        public Guid ReviewsID { get; set; }
        public int ProductsID { get; set; }
        public int UserId { get; set; }
        public required string ReviewTitle { get; set; }
        public required string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserCreated { get; set; }
        public ProductsResponse? Products { get; set; }
    }
}
