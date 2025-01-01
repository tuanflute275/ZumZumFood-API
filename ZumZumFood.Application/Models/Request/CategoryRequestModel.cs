using Microsoft.AspNetCore.Http;

namespace ZumZumFood.Application.Models.Request
{
    public class CategoryRequestModel
    {
        public IFormFile? ImageFile { get; set; }
        public string? OldImage { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? Description { get; set; }
    }
}
