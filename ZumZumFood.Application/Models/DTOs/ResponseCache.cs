namespace ZumZumFood.Application.Models.DTOs
{
     public class ResponseCache
    {
        public List<ProductDTO> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
