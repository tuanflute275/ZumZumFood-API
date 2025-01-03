namespace ZumZumFood.Application.Models.Request
{
    public class ProductImageModel
    {
        public int ProductId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }
}
