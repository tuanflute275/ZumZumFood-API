namespace ZumZumFood.Application.Models.Request
{
    public class ProductImageUpdateModel
    {
        public IFormFile ImageFile { get; set; }
        public string? UpdateBy { get; set; }
    }
}
