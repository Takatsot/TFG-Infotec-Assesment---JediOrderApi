using Core.Models.Domain;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Models.Domain
{
    public class Picture : BaseEntity
    {
        [NotMapped]
        public required IFormFile File { get; set; }
        public required string FileName { get; set; }
        public  string? Description { get; set; }
        public required string Extension { get; set; }
        public long SizeInBytes { get; set; }
        public required string Path { get; set; }

        // Optional navigation property to the product
        public int? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
