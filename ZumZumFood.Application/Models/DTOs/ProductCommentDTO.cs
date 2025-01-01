namespace ZumZumFood.Application.Models.DTOs
{
    public class ProductCommentDTO
    {
        public int ProductCommentId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public UserProductDTO Users { get; set; }
    }
}
