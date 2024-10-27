namespace Core.Models.DTO
{
    public class ProductsResponse
    {
        public int ProductsID { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; } // Precision and scale will be handled in DbContext
        public string? ImageUrl { get; set; }
        public required string Type { get; set; } // To differentiate between Dishes and Drinks
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public required string UserCreated { get; set; }
        public required string UserModified { get; set; }
    }
}
