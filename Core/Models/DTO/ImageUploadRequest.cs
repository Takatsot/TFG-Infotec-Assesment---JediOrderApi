using Microsoft.AspNetCore.Http;

namespace Core.Models.DTO
{
    public class ImageUploadRequest
    {
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
    }
}
